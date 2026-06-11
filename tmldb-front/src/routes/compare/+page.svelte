<script lang="ts">
	import { afterNavigate, goto } from "$app/navigation";
	import type { ModData, SearchResult } from "$lib";
	import LoadingSpinner from "$lib/components/LoadingSpinner.svelte";
	import ModSearch from "$lib/ModSearch.svelte";
	import { onMount } from "svelte";
	import ModComparison from "./ModComparison.svelte";
	import { page } from "$app/state";

	let mod1: Promise<ModData> | null = $state(null)
	let mod2: Promise<ModData> | null = $state(null)

	async function fetchModInfo(id: string): Promise<ModData> {
		return await (await fetch(`/api/mod_info/${id}`)).json();
	}

	function resetModData() {
		const searchParams = page.url.searchParams
		const mod1Param = searchParams.get("mod1");
		if (mod1Param !== null && mod1Param !== "") {
			mod1 = fetchModInfo(mod1Param)
		}

		const mod2Param = searchParams.get("mod2");
		if (mod2Param !== null && mod2Param !== "") {
			mod2 = fetchModInfo(mod2Param)
		}
	}

	onMount(() => {
		resetModData()
	})
</script>

<div id="compare-page">
	<div>
		<ModSearch searchMod={true}>
			{#snippet resultContent(searchResults: SearchResult[])}
				{#each searchResults as res}
					<button onclick={() => {
						const params = new URLSearchParams(page.url.searchParams)
						params.set("mod1", res.id)
						goto(`?${params.toString()}`)
						mod1 = fetchModInfo(res.id)
					}}>{res.name}</button>
				{/each}
			{/snippet}
		</ModSearch>
		{#if mod1}
			{#await mod1}
				<LoadingSpinner size=24></LoadingSpinner>
			{:then mod}
				<ModComparison {mod} />
			{/await}
		{:else}
			<p class="select-msg">Select mod to compare</p>
		{/if}
	</div>
	<div>
		<ModSearch searchMod={true}>
			{#snippet resultContent(searchResults: SearchResult[])}
				{#each searchResults as res}
					<button onclick={() => {
						const params = new URLSearchParams(page.url.searchParams)
						params.set("mod2", res.id)
						goto(`?${params.toString()}`)
						mod2 = fetchModInfo(res.id)
					}}>{res.name}</button>
				{/each}
			{/snippet}
		</ModSearch>
		{#if mod2}
			{#await mod2}
				<LoadingSpinner size=24></LoadingSpinner>
			{:then mod}
				<ModComparison {mod} />
			{/await}
		{:else}
			<p class="select-msg">Select mod to compare</p>
		{/if}
	</div>
</div>

<style>
#compare-page {
	padding: 1rem clamp(1rem, 3vw, 4rem) 0;
	display: grid;
	height: calc(100vh - var(--nav-height) - var(--footer-height) - 3rem);
	grid-template-columns: 1fr 1fr;
	gap: 3rem;
}

#compare-page > div {
	display: flex;
	flex-direction: column;
	gap: 1rem;
}

button {
	clip-path: none;
	&::after {
		display: none;
	}
}

.select-msg {
	text-align: center;
}
</style>
