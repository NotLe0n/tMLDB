using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Numerics;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace tmldb_back;

[Route("api")]
public class DbController : Controller
{
	[HttpGet("global_stats/")]
	public async Task<GlobalStatsResponse> GetGlobalStats()
	{
		await using var conn = await Program.OpenConnection();

		return await conn.QueryFirstAsync<GlobalStatsResponse>(
			"""
			SELECT 
			    COUNT(mod_id) as mods,
			    SUM(downloads_total) as downloads,
			    SUM(views) as views,
			    COUNT(DISTINCT author_id) as creators
			from mods
			""");
	}

	[HttpGet("mod_histogram/")]
	public async Task<ModHistogramResponse> GetModHistogram()
	{
		await using var conn = await Program.OpenConnection();

		return await conn.QueryFirstAsync<ModHistogramResponse>(
			"""
			WITH weekly AS (
				SELECT
					date_bin(
						interval '1 week',
						to_timestamp(time_created) AT TIME ZONE 'UTC',
						timestamp '2021-07-11 12:31:03'
					) AS week_start,
					count(*) AS mods
				FROM mods
				GROUP BY 1
			)
			SELECT
				array_agg(mods      ORDER BY week_start) AS mods,
				array_agg(week_start ORDER BY week_start) AS dates
			FROM weekly;
			""");
	}

	[HttpGet("top_mods/")]
	public async Task<IEnumerable<TopModsResponse>> GetTopMods()
	{
		await using var conn = await Program.OpenConnection();

		return await conn.QueryAsync<TopModsResponse>(
			"""
			SELECT 
				display_name, internal_name, description, workshop_icon_url as icon 
			FROM mods
			ORDER BY downloads_total DESC
			LIMIT 12
			""");
	}

	[HttpGet("top_creators/")]
	public async Task<IEnumerable<TopCreatorsResponse>> GetTopCreators()
	{
		await using var conn = await Program.OpenConnection();

		var res = await conn.QueryAsync<TopCreatorsResponse>(
			"""
			SELECT 
				SUM(downloads_total) as downloads,
				COUNT(mod_id) as mod_count,
				author_id::text,
				mode() WITHIN GROUP ( ORDER BY author ) as author_name
			FROM mods 
			GROUP BY author_id
			ORDER BY SUM(downloads_total)
			DESC LIMIT 12
			""");

		var topCreatorsResponses = res as TopCreatorsResponse[] ?? res.ToArray();
		string ids = string.Join("&steamids=", topCreatorsResponses.Select(x => x.AuthorId));
		string requestUrl = $"https://tmlapis.le0n.dev/1.4/get_steam_avatar?steamids={ids}"; 
		
		using var client = new HttpClient();
		var avatars = await client.GetFromJsonAsync<Dictionary<string, string>>(requestUrl);
		
		foreach (var topCreatorsResponse in topCreatorsResponses) {
			topCreatorsResponse.Avatar = avatars?[topCreatorsResponse.AuthorId];
		}

		return topCreatorsResponses;
	}
	
	[HttpGet("mod_info/{modId:long}")]
	public async Task<object?> GetModInfo(long modId)
	{
		using var client = new HttpClient();
		return await client.GetFromJsonAsync<object>($"https://tmlapis.le0n.dev/1.4/mod/{modId}");
	}
	
	[HttpGet("mod_info/{modName}")]
	public async Task<object?> GetModInfo(string modName)
	{
		using var client = new HttpClient();
		return await client.GetFromJsonAsync<object>($"https://tmlapis.le0n.dev/1.4/mod/{modName}");
	}
	
	[HttpGet("creator_info/{steamId:long}")]
	public async Task<object?> GetCreatorInfo(long steamId)
	{
		using var client = new HttpClient();
		return await client.GetFromJsonAsync<object>($"https://tmlapis.le0n.dev/1.4/author/{steamId}");
	}
	
	[HttpGet("creator_info/{steamName}")]
	public async Task<object?> GetCreatorInfo(string steamName)
	{
		using var client = new HttpClient();
		return await client.GetFromJsonAsync<object>($"https://tmlapis.le0n.dev/1.4/author/{steamName}");
	}

	private static T[] Diff<T>(T[] xs) where T : ISubtractionOperators<T, T, T>, IAdditiveIdentity<T, T>
	{
		if (xs.Length == 0) return [];

		var d = new T[xs.Length];
		d[0] = T.AdditiveIdentity;

		for (int i = 1; i < xs.Length; i++)
			d[i] = xs[i] - xs[i - 1];

		return d[1..];
	}
	
	[HttpGet("mod_history/{modId:long}")]
	public async Task<ModHistoryResponse> GetModHistory(long modId)
	{
		await using var conn = await Program.OpenConnection();
		var res = await conn.QueryFirstAsync<ModHistoryResponse>(
			"""
			SELECT
				array_agg(date) as dates,
			    array_agg(downloads_total) as downloads,
			    array_agg(views) as views,
			    array_agg(favorited) as favorited, 
			    array_agg(playtime) as playtime, 
			    array_agg(votes_up) as votes_up, 
			    array_agg(votes_down) as votes_down,
			    array_agg(DISTINCT time_updated) as time_updated,
			    array_agg(DISTINCT version) as version
			FROM mod_history
			WHERE mod_id = @modId
			""", new { modId });

		if (res.Dates.Length == 0) {
			return new ModHistoryResponse();
		}
		
		res.Dates = res.Dates[1..];
		res.Downloads = Diff(res.Downloads);
		res.Views = Diff(res.Views);
		res.Favorited = Diff(res.Favorited);
		res.Playtime = Diff(res.Playtime);
		res.VotesUp = Diff(res.VotesUp);
		res.VotesDown = Diff(res.VotesDown);
		return res;
	}

	[HttpGet("creator_history/{steamId:long}")]
	public async Task<CreatorHistoryResponse> GetCreatorHistory(long steamId)
	{
		await using var conn = await Program.OpenConnection();
		
		var lines = await conn.QueryAsync<CreatorHistoryLine>(
			"""
			SELECT
			    mod_id as label,
			    array_agg(downloads_total ORDER BY date) as data
			FROM mod_history
			WHERE author_id = @steamId
			GROUP BY mod_id
			""", new { steamId });
		
		foreach (var data in lines) {
			data.Data = Diff(data.Data);
		}
		
		var dates = await conn.QueryFirstAsync<DateOnly[]>(
			"""
			SELECT
			    array_agg(DISTINCT date) as dates
			FROM mod_history
			WHERE author_id = @steamId
			""", new { steamId });
		
		return new CreatorHistoryResponse {
			Dates = dates[1..],
			Lines = lines
		};
	}
	
	[HttpGet("mod_list/{page:int}")]
	public async Task<IEnumerable<ModListResponse>> GetModList(
		int page, 
		[FromQuery] bool desc,
		[FromQuery] ModListOrder sort = ModListOrder.DownloadsTotal,
		[FromQuery] string[]? modSideFilters = null,
		[FromQuery] string[]? tagFilters = null,
		[FromQuery] string[]? versionFilters = null,
		[FromQuery] string search = "")
	{
		const int PageSize = 50;
		int offset = page * PageSize;
		
		string orderByColumn = sort switch
		{
			ModListOrder.DownloadsTotal => "downloads_total",
			ModListOrder.Favorited      => "favorited",
			ModListOrder.Views          => "views",
			ModListOrder.Score          => "score",
			ModListOrder.TimeCreated    => "time_created",
			ModListOrder.TimeUpdated    => "time_updated",
			_ => "downloads_total"
		};
		string sortOrder = desc ? "DESC" : "ASC";
		bool shouldFilterZeroScore = sort == ModListOrder.Score && !desc;
		
		await using var conn = await Program.OpenConnection();
		return await conn.QueryAsync<ModListResponse>(
			$"""
			SELECT 
				mod_id,
				display_name,
				author,
				description, 
				downloads_total, 
				views, 
				favorited, 
				score,
				votes_up, 
				votes_down,
				playtime,
				workshop_icon_url,
				time_created,
				time_updated
			FROM mods m
			WHERE (@modSideFilters::text[] IS NULL OR modside=ANY(@modSideFilters::text[]))
			AND (@tagFilters::text[] IS NULL OR EXISTS (
				SELECT 1
				FROM mod_tags mt
				WHERE (mt.mod_id = m.mod_id
					AND mt.tag = ANY(@tagFilters::text[]))
			))
			AND (@versionFilters::text[] IS NULL OR EXISTS (
				SELECT 1
				FROM mod_versions mv
				WHERE (mv.mod_id = m.mod_id
					AND mv.tmodloader_version = ANY(@versionFilters::text[]))
			))
			AND (@search = '' OR display_name LIKE '%' || @search || '%')
			AND (NOT @shouldFilterZeroScore OR score != 0)
			ORDER BY {orderByColumn} {sortOrder}
			LIMIT @PageSize OFFSET @offset
			""", new {
				PageSize, offset, search, modSideFilters, tagFilters, versionFilters, shouldFilterZeroScore
			});
	}

	[HttpGet("mod_filters/")]
	public async Task<ModFiltersResponse> GetModFilters()
	{
		await using var conn = await Program.OpenConnection();
		return await conn.QueryFirstAsync<ModFiltersResponse>(
			"""
			SELECT 
			    array_agg(DISTINCT modside) as mod_sides,
			    array_agg(DISTINCT mod_tags.display_name) as tags,
			    array_agg(DISTINCT tmodloader_version ORDER BY tmodloader_version DESC) as tml_versions
			FROM mods JOIN mod_tags USING (mod_id) JOIN mod_versions USING (mod_id)
			""");
	}

	[HttpGet("creator_names/{steamId:long}")]
	public async Task<IEnumerable<string>> GetCreatorNames(long steamId)
	{
		await using var conn = await Program.OpenConnection();
		var res = await conn.QueryAsync<string>(
			"""
			SELECT DISTINCT author FROM mods 
			WHERE author_id = @steamId
			""", 
			new { steamId });

		// filter names containing previous occurrences
		var names = new List<string>();
		foreach (var item in res.OrderBy(x => x.Length)) {
			if (!names.Any(n => item.Contains(n))) {
				names.Add(item);
			}
		}

		return names;
	}

	[HttpGet("creator_list/{page:int}")]
	public async Task<IEnumerable<CreatorListResponse>> GetCreatorList(
		int page,
		[FromQuery] bool desc,
		[FromQuery] CreatorListOrder sort = CreatorListOrder.DownloadsTotal,
		[FromQuery] string search = "") 
	{
		const int PageSize = 100;
		int offset = page * PageSize;
		
		string orderByColumn = sort switch
		{
			CreatorListOrder.ModCount       => "COUNT(mod_id)",
			CreatorListOrder.DownloadsTotal => "SUM(downloads_total)",
			CreatorListOrder.Favorited      => "SUM(favorited)",
			CreatorListOrder.Views          => "SUM(views)",
			_ => "SUM(downloads_total)"
		};
		
		string sortOrder = desc ? "DESC" : "ASC";
		
		await using var conn = await Program.OpenConnection();
		var creatorListResponse = await conn.QueryAsync<CreatorListResponse>(
			$"""
			SELECT
			    author_id::text,
			    mode() WITHIN GROUP ( ORDER BY author ) as author_name,
			    COUNT(mod_id) as mod_count,
			    SUM(downloads_total) as downloads,
			    SUM(views) as views,
			    SUM(favorited) as favorites
			FROM mods
			WHERE (@search = '' OR author LIKE '%' || @search || '%')
			GROUP BY author_id
			ORDER BY {orderByColumn} {sortOrder}
			LIMIT @PageSize OFFSET @offset
			""", new { PageSize, offset, @search });
		
		string ids = string.Join("&steamids=", creatorListResponse.Select(x => x.AuthorId));
		string requestUrl = $"https://tmlapis.le0n.dev/1.4/get_steam_avatar?steamids={ids}";
		
		using var client = new HttpClient();
		var avatars = await client.GetFromJsonAsync<Dictionary<string, string>>(requestUrl);
		
		foreach (var creator in creatorListResponse) {
			creator.Avatar = avatars?[creator.AuthorId];
		}

		return creatorListResponse;
	}

	[HttpGet("search_mod/")]
	public async Task<IEnumerable<SearchResult>> GetModSearch(string? query)
	{
		if (query is null || query.Length <= 1) {
			return [];
		}
		
		await using var conn = await Program.OpenConnection();
		return conn.Query<SearchResult>(
			"""
			SELECT display_name as name, mod_id::text as id FROM mods
			WHERE 
				LOWER(display_name) LIKE '%' || LOWER(@query) || '%'
				OR internal_name LIKE '%' || LOWER(@query) || '%'
			ORDER BY downloads_total DESC
			LIMIT 50;
			""", new { query });
	}

	[HttpGet("search_creator/")]
	public async Task<IEnumerable<SearchResult>> GetCreatorSearch(string? query)
	{
		if (query is null || query.Length <= 1) {
			return [];
		}
		
		await using var conn = await Program.OpenConnection();
		return conn.Query<SearchResult>(
			"""
			SELECT author as name, author_id::text as id FROM mods
			WHERE 
				LOWER(author) LIKE '%' || LOWER(@query) || '%'
			GROUP BY author, author_id
			ORDER BY SUM(downloads_total) DESC
			LIMIT 50;
			""", new { query });
	}
}