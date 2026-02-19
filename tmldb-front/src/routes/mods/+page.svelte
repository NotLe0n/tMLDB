<script lang="ts">
	import type { ModListData } from "$lib";
	import { mdiClockEdit, mdiClockPlus, mdiDownload, mdiEye, mdiStar, mdiThumbsUpDown } from "@mdi/js";

	import FilterSelector from "./FilterSelector.svelte";
	import ModListCard from "$lib/components/ModListCard.svelte";
	import SortSelector from "$lib/components/SortSelector.svelte";
	import ScrollDetector from "$lib/components/ScrollDetector.svelte";
	import { onMount } from "svelte";
	import { page } from "$app/state";
	import { goto } from "$app/navigation";
	import ListPage from "$lib/components/page-components/ListPage.svelte";

	const DEFAULT_SEARCH = ""
	const DEFAULT_SORT = 0
	const DEFAULT_ORDER = true
	const DEFAULT_FILTER: string[] = []

	let loading = $state(false)
	let modList = $state<ModListData[]>([])
	let currentPage = $state(0)
	let searchQuery = $state(DEFAULT_SEARCH)
	let sortSelection = $state(DEFAULT_SORT)
	let sortDesc = $state(DEFAULT_ORDER)
	let modSideFilter = $state<string[]>(DEFAULT_FILTER)
	let tagsFilter = $state<string[]>(DEFAULT_FILTER)
	let versionsFilter = $state<string[]>(DEFAULT_FILTER)

	onMount(() => {
		const searchParams = page.url.searchParams
		searchQuery = searchParams.get("search") ?? DEFAULT_SEARCH
		sortSelection = Number.parseInt(searchParams.get("sort") ?? DEFAULT_SORT.toString())
		sortDesc = (searchParams.get("desc") ?? DEFAULT_ORDER.toString()) == "true"
		modSideFilter = searchParams.getAll("modSide")
		tagsFilter = searchParams.getAll("tags")
		versionsFilter = searchParams.getAll("versions")
	})

	const sortOptions = [
		{id: "download",     path: mdiDownload,     title: "Downloads"},
		{id: "favorites",    path: mdiStar,         title: "Favorites"},
		{id: "views",        path: mdiEye,          title: "Views"},
		{id: "score",        path: mdiThumbsUpDown, title: "Score"},
		{id: "time-created", path: mdiClockPlus,    title: "Time Created"},
		{id: "time-updated", path: mdiClockEdit,    title: "Time Updated"}
	]

	const fetchMods = async (): Promise<ModListData[]> => {
		//console.log("fetching mod list")
		const modSideFilterStr = modSideFilter.map(x => `modSideFilters=${x}`).join('&')
		const tagFilterStr = tagsFilter.map(x => `tagFilters=${x}`).join('&')
		const versionFilterStr = versionsFilter.map(x => `versionFilters=${x}`).join('&')

		return await ((await fetch(
			`/api/mod_list/${currentPage}?desc=${sortDesc}&sort=${sortSelection}&${modSideFilterStr}&${tagFilterStr}&${versionFilterStr}&search=${searchQuery}`
		)).json())
	}

	const updateModList = async () => {
		const params = new URLSearchParams()
		if (searchQuery != DEFAULT_SEARCH) params.set("search", searchQuery)
		if (sortSelection != DEFAULT_SORT) params.set("sort", sortSelection.toString())
		if (sortDesc != DEFAULT_ORDER) params.set("desc", "false")
		modSideFilter.forEach(x => params.append("modSide", x))
		tagsFilter.forEach(x => params.append("tags", x))
		versionsFilter.forEach(x => params.append("versions", x))
		goto(`?${params.toString()}`)

		currentPage = 0
		loading = true
		const mods = await fetchMods()
		modList = mods
		currentPage++
		loading = false
	}

	async function onScrollDetect() {
		const mods = await fetchMods();
		modList.push(...mods)
		currentPage++
	}

	function resetParams() {
		searchQuery = DEFAULT_SEARCH
		sortSelection = DEFAULT_SORT
		sortDesc = DEFAULT_ORDER
		modSideFilter = DEFAULT_FILTER
		tagsFilter = DEFAULT_FILTER
		versionsFilter = DEFAULT_FILTER
		updateModList()
	}
</script>

<svelte:head>
	<title>tMLDB - Mods</title>
	<meta name="description" content="View a list of all Terraria mods">
</svelte:head>

<ListPage onsubmit={updateModList} bind:searchQuery={searchQuery} resetControls={resetParams}>
	{#snippet controls()}
		<div>
			<SortSelector 
				sortOptions={sortOptions}
				bind:selectedIdx={sortSelection} 
				bind:sortDesc={sortDesc} 
				onchange={updateModList} 
			/>
		</div>
		<div>
			<FilterSelector 
				bind:modSides={modSideFilter}
				bind:tags={tagsFilter}
				bind:versions={versionsFilter}
				onapply={updateModList} 
			/>
		</div>
	{/snippet}

	{#snippet list()}
		<div id="modlist">
			{#each modList as mod}
				<ModListCard {mod} />
			{/each}
			<ScrollDetector bind:loading={loading} onDetect={onScrollDetect} />
		</div>
	{/snippet}
</ListPage>

<style>
	#modlist {
		overflow-y: auto;
		display: flex;
		flex-direction: column;
		gap: 1rem;
		padding-right: 0.5rem;
	}
</style>