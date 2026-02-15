<script lang="ts">
	import ModsHistogram from "./ModsHistogram.svelte";
	import ModCard from "./ModCard.svelte";
	import StatCard from "$lib/StatCard.svelte";
	import CreatorCard from "./CreatorCard.svelte";
	import LinkButton from "$lib/LinkButton.svelte";
	import LoadingSpinner from "$lib/LoadingSpinner.svelte";
	import { mdiDownload, mdiEye, mdiPackageVariant, mdiAccount } from "@mdi/js";

	type GlobalStatsResponse = { mods: number, downloads: number, views: number, creators: number }
	type TopModsResponse = { display_name: string, internal_name: string, description: string, icon: string }
	type TopCreatorsResponse = { downloads: number, mod_count: number, author_id: string, author_name: string, avatar: string }
	type ModHistogramResponse = { dates: string[], mods: number[] }

	async function getGlobalStats(): Promise<GlobalStatsResponse> {
		return await (await fetch("/api/global_stats")).json();
	}

	async function getTopMods(): Promise<TopModsResponse[]> {
		return await (await fetch("/api/top_mods")).json();
	}

	async function getTopCreators(): Promise<TopCreatorsResponse[]> {
		return await (await fetch("/api/top_creators")).json();
	}

	async function getModHistogram(): Promise<ModHistogramResponse> {
		return await (await fetch("/api/mod_histogram")).json();
	}

	let searchQuery = $state("");
</script>

<div id="home-container">
	<section id="hero">
		<h1>tModLoader Database</h1>
		<p class="tagline">The place to look up Terraria mods</p>
	</section>

	<section id="stats-section">
		{#await getGlobalStats()}
			<div>
				<LoadingSpinner size=24 />
			</div>
		{:then global_stats}
			<div id="global-stat-cards">
				<StatCard label="Total Mod Count" value={global_stats.mods} icon={mdiPackageVariant} />
				<StatCard label="Total Mod Downloads" value={global_stats.downloads} icon={mdiDownload} />
				<StatCard label="Total Mod Views" value={global_stats.views} icon={mdiEye} />
				<StatCard label="Total Mod Creators" value={global_stats.creators} icon={mdiAccount} />
			</div>
		{/await}

		<div id="chart-container">
			<h3>Mod count over time</h3>
			<div id="chart-wrapper">
				{#await getModHistogram()}
					<LoadingSpinner size=24 />
				{:then mod_histogram} 
					<ModsHistogram title="Weekly mod count" data={{
						labels: [...mod_histogram.dates],
						data: [...mod_histogram.mods]
					}} />
				{/await}
			</div>
		</div>
	</section>

	<section id="top-mods-section">
		<div class="section-header">
			<h2>Top Mods</h2>
			<LinkButton link="/mods" title="View All Mods →" />
		</div>
		<div id="top-mods">
			{#await getTopMods()}
				<LoadingSpinner size=16 />
			{:then top_mods} 
				{#each top_mods as mod}
					<ModCard {mod} />
				{/each}   
			{/await}
		</div>
	</section>

	<section id="top-creators-section">
		<div class="section-header">
			<h2>Top Creators</h2>
			<LinkButton link="/creators" title="View All Creators →" />
		</div>
		<div id="top-creators">
			{#await getTopCreators()}
				<LoadingSpinner size=16 />
			{:then top_creators} 
				{#each top_creators as creator}
					<CreatorCard 
						id={creator.author_id}
						name={creator.author_name}
						modCount={creator.mod_count}
						totalDownloads={creator.downloads}
						avatar={creator.avatar}
					/>
				{/each}
			{/await}
			
		</div>
	</section>
</div>

<style>
#home-container {
	display: flex;
	align-items: center;
	flex-direction: column;
	gap: 2rem;
	width: 90%;
	max-width: 1400px;
	margin: 4rem auto;
}

section {
	width: 100%;

	h2 {
		text-align: center;
		font-size: 2.5rem;

		@media (max-width: 520px) {
			font-size: 2rem;
		}
	}
}

#hero {
	text-align: center;
	width: 100%;

	h1 {
		font-size: 3.5rem;
		margin-bottom: 0.5rem;
		background: var(--gradient);
		background-clip: text;
		-webkit-text-fill-color: transparent;

		@media (max-width: 435px) {
			font-size: 3rem;
		}
	}

	.tagline {
		font-size: 1.3rem;
		opacity: 0.8;
		margin-bottom: 2rem;
	}
}

#global-stat-cards {
	display: flex;
	flex-wrap: wrap;
	justify-content: center;
	gap: 1.5rem;
	width: 100%;
}

#chart-container {
	padding: 2rem;
	background-color: rgba(31, 44, 31, 0.3);
	border: 1px solid var(--primary);
	border-radius: 1rem;
	margin: 2rem auto;
}

#chart-wrapper {
	width: 100%;
	height: 300px;
}

.section-header {
	display: flex;
	justify-content: space-between;
	align-items: center;
	margin-bottom: 2rem;
	gap: .5rem;

	@media (max-width: 435px) {
		flex-direction: column;
	}
}

#top-mods {
	display: grid;
	grid-template-columns: repeat(auto-fill, minmax(290px, 1fr));
	gap: 1.5rem;
	width: 100%;
}

#top-creators {
	display: grid;
	grid-template-columns: repeat(auto-fill, minmax(290px, 1fr));
	gap: 1rem;
	width: 100%;
}
</style>