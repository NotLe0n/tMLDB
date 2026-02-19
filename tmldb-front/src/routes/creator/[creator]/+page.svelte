<script lang="ts">
	import type { ModData } from "$lib";
	import ModListCard from "$lib/components/ModListCard.svelte";
	import LoadingSpinner from "$lib/components/LoadingSpinner.svelte";
	import CreatorHistoryChart from "./CreatorHistoryChart.svelte";
	import CreatorHeader from "./CreatorHeader.svelte";
	import type { PageData } from './$types';
	import StatPage from "$lib/components/page-components/StatPage.svelte";

	let { data }: { data: PageData } = $props();
	const creator = $derived(data.creator);

	function getModNameMap(mods: ModData[]) {
		return new Map(mods.map(m => [m.mod_id, m.display_name]))
	}
</script>

<svelte:head>
	<title>tMLDB - {creator.steam_name}</title>
	<meta name="description" content="View information about the Terraria modder '{creator.steam_name}' including statistics, download history, and mod dependencies.">
	<meta name="theme-color" content="#1b8d1b">

	<meta property="og:type" content="website" />
	<meta property="og:title" content="tMLDB - {creator.steam_name}" />
	<meta property="og:description" content="View information about the Terraria modder '{creator.steam_name}' including statistics, download history, and mod dependencies." />
	<meta property="og:image" content="https://tml-card.le0n.dev/?steamid64={creator.steam_id}" />

	<meta property="twitter:card" content="summary_large_image" />
	<meta property="twitter:title" content="tMLDB - {creator.steam_name}" />
	<meta property="twitter:description" content="View information about the Terraria modder '{creator.steam_name}' including statistics, download history, and mod dependencies." />
	<meta property="twitter:image" content="https://tml-card.le0n.dev/?steamid64={creator.steam_id}" />
</svelte:head>

<StatPage>
	<CreatorHeader {creator} />

	<article id="mods-list">
		<h3>Mods by {creator.steam_name}</h3>
		<div id="mods-container">
			{#each creator.mods as mod}
				<ModListCard {mod} />
			{/each}
		</div>
	</article>

	{#await data.history}
		<LoadingSpinner size=24 />
	{:then creatorHistory}
		{#if creatorHistory.dates.length > 0}
			<article id="mod-history">
				<h3>Daily change over time</h3>
				<div>
					<CreatorHistoryChart data={creatorHistory} modNameMap={getModNameMap(creator.mods)} />
				</div>
			</article>
		{/if}
	{/await}
</StatPage>

<style>
	#mods-container {
		display: flex;
		flex-direction: column;
		gap: 1rem;
	}
</style>