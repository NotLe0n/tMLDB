<script lang="ts">
	import type { ModData } from "$lib";
	import ModListCard from "$lib/ModListCard.svelte";
	import LoadingSpinner from "$lib/LoadingSpinner.svelte";
	// @ts-ignore
	import CreatorHistoryChart from "./CreatorHistoryChart.svelte";
    import CreatorHeader from "./CreatorHeader.svelte";
    import type { PageData } from './$types';

	let { data }: { data: PageData } = $props();
	const creator = $derived(data.creator);

	function getModNameMap(mods: ModData[]) {
		return new Map(mods.map(m => [m.mod_id, m.display_name]))
	}
</script>

<div id="page">
	<CreatorHeader {creator} />

	<article id="mods-list">
		<h2>Mods by {creator.steam_name}</h2>
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
					<h2>Daily change over time</h2>
					<div>
						<CreatorHistoryChart data={creatorHistory} modNameMap={getModNameMap(creator.mods)} />
					</div>
				</article>
			{/if}
		{/await}
</div>

<style>
	#page {
		padding: clamp(2rem, 4vw, 4rem);
		display: flex;
		flex-direction: column;
		gap: 2.5rem;

		--avatar-size: 176px;
	}

	article {
		padding: 1.5rem;
		border-radius: 1rem;
		background: var(--tertiary-bg);
		border: 1px solid rgba(132, 204, 22, 0.15);

		h2 {
			margin-bottom: 1.5rem;
			font-size: 1.8rem;
		}
	}

	#mods-container {
		display: flex;
		flex-direction: column;
		gap: 1rem;
	}
</style>