<script lang="ts">
	import bbobHTML from '@bbob/html'
	import presetHTML5 from '@bbob/preset-html5'

	import ModHistoryChart from './ModHistoryChart.svelte'
	import LoadingSpinner from '$lib/components/LoadingSpinner.svelte';
	import ModListCard from '$lib/components/ModListCard.svelte';
	import ModHeader from './ModHeader.svelte';
	import type { PageData } from './$types';
	import StatPage from '$lib/components/page-components/StatPage.svelte';
	import DateRangeSelector from '../../../lib/components/DateRangeSelector.svelte';
	import ModStatTable from './ModStatTable.svelte';
	import ToggleChip from '$lib/components/ToggleChip.svelte';

	let { data }: { data: PageData } = $props();
	const mod = $derived(data.mod);
	let beginDate = $state("")
	let endDate = $state("")
	let chartView = $state(true)

	function descriptionToHTML(description: string) {
		return bbobHTML(description, presetHTML5()).trim()
	}
</script>

<svelte:head>
	<title>tMLDB - {mod.display_name}</title>
	<meta name="description" content="View information about the {mod.display_name} mod for tModLoader including statistics, download history, and mod dependencies.">
	<meta name="theme-color" content="#1b8d1b">

	<meta property="og:type" content="website" />
	<meta property="og:title" content="tMLDB - {mod.display_name}" />
	<meta property="og:description" content="View information about the {mod.display_name} mod for tModLoader including statistics, download history, and mod dependencies." />
	<meta property="og:image" content="https://tml-card.le0n.dev/?modname={mod.mod_id}" />

	<meta property="twitter:card" content="summary_large_image" />
	<meta property="twitter:title" content="tMLDB - {mod.display_name}" />
	<meta property="twitter:description" content="View information about the {mod.display_name} mod for tModLoader including statistics, download history, and mod dependencies." />
	<meta property="twitter:image" content="https://tml-card.le0n.dev/?modname={mod.mod_id}" />
</svelte:head>

<StatPage>
	<ModHeader {mod} />

	<article id="description" class="panel">
		<pre>{@html descriptionToHTML(mod.description ?? "")}</pre>
	</article>

	{#await data.history}
		<LoadingSpinner size=24 />
	{:then mod_history}
		{#if mod_history.dates.length > 0}
			<article id="mod-history" class="panel">
				<header>
					<h2>Daily change over time</h2>
					<ToggleChip bind:value={chartView} label1="Chart" label2="Table" />
				</header>

				{#if chartView}
					<div><ModHistoryChart data={mod_history} {beginDate} {endDate} /></div>
				{:else}
					<div id="table-container"><ModStatTable data={mod_history} {beginDate} {endDate} /></div>
				{/if}

				<DateRangeSelector dates={mod_history.dates} bind:beginDate bind:endDate />
			</article>
		{/if}
	{/await}

	{#if mod.children || mod.mod_references != ''}
		<article id="cnr" class="panel">
			<h2>Children and References</h2>

			<h3>References</h3>
			<div id="references">
				{#each mod.mod_references.split(", ") as ref}
					<a href={ref} class="reference panel">{ref}</a>
				{:else}
					<p>No References</p>
				{/each}
			</div>
			<h3>Children</h3>
			<div id="children">
				{#await data.children }
					<LoadingSpinner size=24 />
				{:then children}
					{#each children as child }
						<ModListCard mod={child} />
					{:else}
						<p>No Children</p>
					{/each}
				{/await}
			</div>
		</article>
	{/if}
</StatPage>

<style>
	#description {
		padding-right: 1rem;
		pre {
			padding-right: 0.25rem;
			padding-bottom: 0.25rem;
			overflow: auto;
			max-height: 40rem;
			text-wrap: auto;
			font-size: 0.99rem;
			line-height: 1.1;
			color: rgba(255, 255, 255, 0.82);
		}
	}

	:global(pre#description img) {
		max-width: 50%;
	}

	#mod-history > header {
		display: flex;
		justify-content: space-between;
		align-items: center;
	}

	#cnr {
		display: flex;
		flex-direction: column;
		gap: 1rem;
	}

	#references {
		display: flex;
		gap: .5rem;
	}

	.reference {
		background-color: var(--panel-col2);
		padding: 0.5rem;

		&:hover {
			--panel-border: var(--yellow);
		}
	}

	#children {
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
	}

	#table-container {
		overflow-x: auto;
	}
</style>
