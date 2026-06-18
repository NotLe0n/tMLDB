<script lang="ts">
	import { goto } from "$app/navigation";
	import type { ModData, SearchResult } from "$lib";
	import LoadingSpinner from "$lib/components/LoadingSpinner.svelte";
	import ModSearch from "$lib/ModSearch.svelte";
	import { onMount } from "svelte";
	import ModComparison from "./ModComparison.svelte";
	import { page } from "$app/state";

	let mod1: ModData | null = $state(null)
	let mod2: ModData | null = $state(null)
	let loading1 = $state(false)
	let loading2 = $state(false)

	let currentMod1Promise: Promise<ModData> | null = null;
	let currentMod2Promise: Promise<ModData> | null = null;

	async function fetchModInfo(id: string): Promise<ModData> {
		return await (await fetch(`/api/mod_info/${id}`)).json();
	}

	function setMod1(id: string) {
		const p = fetchModInfo(id);
		currentMod1Promise = p;
		loading1 = true;
		mod1 = null;
		p.then(data => {
			if (currentMod1Promise === p) {
				mod1 = data;
				loading1 = false;
			}
		});
	}

	function setMod2(id: string) {
		const p = fetchModInfo(id);
		currentMod2Promise = p;
		loading2 = true;
		mod2 = null;
		p.then(data => {
			if (currentMod2Promise === p) {
				mod2 = data;
				loading2 = false;
			}
		});
	}

	onMount(() => {
		const searchParams = page.url.searchParams;
		const mod1Param = searchParams.get("mod1");
		if (mod1Param) setMod1(mod1Param);
		const mod2Param = searchParams.get("mod2");
		if (mod2Param) setMod2(mod2Param);
	})
</script>

<div id="compare-page">
	<div class="search-row">
		<div>
			<ModSearch searchMod={true}>
				{#snippet resultContent(searchResults: SearchResult[])}
					{#each searchResults as res}
						<button onclick={() => {
							const params = new URLSearchParams(page.url.searchParams)
							params.set("mod1", res.id)
							goto(`?${params.toString()}`)
							setMod1(res.id)
						}}>{res.name}</button>
					{/each}
				{/snippet}
			</ModSearch>
		</div>
		<span>vs</span>
		<div>
			<ModSearch searchMod={true}>
				{#snippet resultContent(searchResults: SearchResult[])}
					{#each searchResults as res}
						<button onclick={() => {
							const params = new URLSearchParams(page.url.searchParams)
							params.set("mod2", res.id)
							goto(`?${params.toString()}`)
							setMod2(res.id)
						}}>{res.name}</button>
					{/each}
				{/snippet}
			</ModSearch>
		</div>
	</div>

	{#if loading1 || loading2}
		<LoadingSpinner size={20} />
	{:else}
		{#if mod1 || mod2}
			<ModComparison {mod1} {mod2} />
		{:else if !loading1 && !loading2}
			<p class="select-msg">Select a mod to compare</p>
		{/if}
	{/if}
</div>

<style>
#compare-page {
	padding: 1rem clamp(1rem, 3vw, 4rem) 0;
	display: flex;
	flex-direction: column;
	height: calc(100vh - var(--nav-height) - var(--footer-height) - 3rem);
	gap: 1rem;
}

.search-row {
	display: grid;
	grid-template-columns: 1fr 1rem 1fr;
	align-items: center;
	gap: 0.2rem clamp(0.1rem, 3vw, 3rem);

	@media (max-width: 815px) {
		& > span {
			display: none;
		}
		grid-template-columns: none;
		grid-template-rows: 1fr 1fr;
	}
}

.search-row > div {
	display: grid;
	grid-template-columns: 1fr auto;
	gap: 0.5rem;
}

button {
	background-color: unset;
	clip-path: none;
	&::after {
		display: none;
	}
}

.select-msg {
	text-align: center;
}
</style>
