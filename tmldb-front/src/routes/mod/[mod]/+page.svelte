<script lang="ts">
	import bbobHTML from '@bbob/html'
	import presetHTML5 from '@bbob/preset-html5'

	import ModHistoryChart from './ModHistoryChart.svelte'
    import LoadingSpinner from '$lib/LoadingSpinner.svelte';
    import ModListCard from '$lib/ModListCard.svelte';
    import ModHeader from './ModHeader.svelte';
    import type { PageData } from './$types';

	let { data }: { data: PageData } = $props();
	const mod = $derived(data.mod);

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

<div id="mod-page">
	<ModHeader {mod} />

	<article id="description">
		<pre>{@html descriptionToHTML(mod.description ?? "")}</pre>
	</article>

	{#await data.history}
		<LoadingSpinner size=24 />
	{:then mod_history} 
		{#if mod_history.dates.length > 0}
			<article id="mod-history">
				<h2>Daily change over time</h2>
				<div>
					<ModHistoryChart data={mod_history} />
				</div>
			</article>
		{/if}
	{/await}

	{#if mod.children || mod.mod_references != ''}
		<article id="cnr">
			<h2>Children and References</h2>

			<h3>References</h3>
			<div id="references">
				{#each mod.mod_references.split(", ") as ref}
					<a href={ref} class="reference">{ref}</a>
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
</div>

<style>
	#mod-page {
		padding: clamp(1rem, 4vw, 4rem);
		display: flex;
		flex-direction: column;
		gap: 2.5rem;

		--icon-size: 176px;
	}

	article {
		padding: 1.5rem;
		border-radius: 1rem;
		background: var(--tertiary-bg);
		border: 1px solid rgba(132, 204, 22, 0.15);

		h2 {
			margin-bottom: 1rem;
		}
	}

	#description {
		padding-right: 1rem;
		max-height: 40rem;
		overflow: auto;

		pre {
			text-wrap: auto;
			font-size: 0.99rem;
			line-height: 1.1;
			color: rgba(255, 255, 255, 0.82);
		}
	}

	:global(pre#description img) {
		max-width: 50%;
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
		background-color: var(--tertiary-bg);
		border-color: var(--primary);
		border-radius: 1rem;
		padding: 0.5rem;

		&:hover {
			background-color: var(--tertiary-hov);
		}
	}

	#children {
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
	}
</style>