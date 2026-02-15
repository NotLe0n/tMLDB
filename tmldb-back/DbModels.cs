using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable NotAccessedPositionalProperty.Global

namespace tmldb_back;

[Table("GlobalStatsResponse")]
public sealed class GlobalStatsResponse
{
	public long Mods { get; init; }
	public long Downloads { get; init; }
	public long Views { get; init; }
	public long Creators { get; init; }
}

[Table("ModHistogramResponse")]
public sealed class ModHistogramResponse
{
	public long[] Mods { get; init; } = [];
	public DateTime[] Dates { get; init; } = [];
}

[Table("TopModsResponse")]
public sealed class TopModsResponse
{
	public string DisplayName { get; init; } = null!;
	public string InternalName { get; init; } = null!;
	public string Description { get; init; } = null!;
	public string Icon { get; init; } = null!;
}

[Table("TopCreatorsResponse")]
public sealed class TopCreatorsResponse
{
	public int Downloads { get; init; }
	public int ModCount { get; init; }
	public string AuthorId { get; init; }
	public string AuthorName { get; init; } = null!;
	public string? Avatar { get; set; }
}

public sealed class ModResponse
{
	public long ModId { get; set; }
	public string? DisplayName { get; set; }
	public string? InternalName { get; set; }
	public string? Author { get; set; }
	public long AuthorId { get; set; }
	public string Modside { get; set; } = null!;
	public string Homepage { get; set; } = null!;
	public string ModReferences { get; set; } = null!;
	public int NumVersions { get; set; }
	public long TimeCreated { get; set; }
	public long TimeUpdated { get; set; }
	public string? WorkshopIconUrl { get; set; }
	public string? Description { get; set; }
	public int DownloadsTotal { get; set; }
	public int Favorited { get; set; }
	public int Followers { get; set; }
	public long Views { get; set; }
	public string? Playtime { get; set; }
	public int NumComments { get; set; }
	public double Score { get; set; }
	public int VotesUp { get; set; }
	public int VotesDown { get; set; }
	public string[]? Tags { get; set; }
	public string? Youtube { get; set; }
	public string? Twitter { get; set; }
	public string? Reddit { get; set; }
	public string? Facebook { get; set; }
	public string? Sketchfab { get; set; }
}

public sealed class ModVersion
{
	public int Id { get; set; }
	public long ModId { get; set; }
	public string ModVersionValue { get; set; } = null!;
	public string TmodloaderVersion { get; set; } = null!;
}

public sealed class ModChild
{
	public long ParentModId { get; set; }
	public long ChildModId { get; set; }
}

public sealed class ModHistoryResponse
{
	public DateOnly[] Dates { get; set; } = [];
	public int[] Downloads { get; set; } = [];
	public long[] Views { get; set; } = [];
	public int[] Favorited { get; set; } = [];
	public long[] Playtime { get; set; } = [];
	public int[] VotesUp { get; set; } = [];
	public int[] VotesDown { get; set; } = [];
	public long[] TimeUpdated { get; set; } = [];
	public string[] Version { get; set; } = [];
}

public enum ModListOrder
{
	DownloadsTotal,
	Views,
	Favorited,
	Score,
	TimeCreated,
	TimeUpdated
}

public sealed class ModListResponse
{
	public long ModId { get; set; }
	public string? DisplayName { get; set; }
	public string? Author { get; set; }
	public string Description { get; set; }
	public int DownloadsTotal { get; set; }
	public int Favorited { get; set; }
	public long Views { get; set; }
	public double Score { get; set; }
	public int VotesUp { get; set; }
	public int VotesDown { get; set; }
	public string Playtime { get; set; }
	public string? WorkshopIconUrl { get; set; }
	public long TimeUpdated { get; set; }
	public long TimeCreated { get; set; }
}

public sealed class ModFiltersResponse
{
	public string[]? ModSides { get; set; }
	public string[]? Tags { get; set; }
	public string[]? TmlVersions { get; set; }
}


public sealed class CreatorHistoryResponse
{
	public DateOnly[] Dates { get; set; } = [];
	public IEnumerable<CreatorHistoryLine> Lines { get; set; } = [];
}

public sealed class CreatorHistoryLine
{
	public long Label { get; set; }
	public int[] Data { get; set; } = [];
}

public enum CreatorListOrder
{
	ModCount,
	DownloadsTotal,
	Views,
	Favorited
}

public sealed class CreatorListResponse
{
	public string AuthorId { get; set; } = null!;
	public string AuthorName { get; set; } = null!;
	public long ModCount { get; set; }
	public long Downloads { get; set; }
	public long Views { get; set; }
	public long Favorites { get; set; }
	public string? Avatar { get; set; }
}

public sealed class SearchResult
{
	public string name { get; set; } = null!;
	public string id { get; set; } = null!;
}