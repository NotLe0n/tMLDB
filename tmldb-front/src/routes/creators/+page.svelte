<script lang="ts">
	import { type CreatorListData } from "$lib"
	import { mdiDownload, mdiStar, mdiEye, mdiPackageVariant } from "@mdi/js";
	import SortSelector from "$lib/components/SortSelector.svelte";
	import CreatorListCard from "./CreatorListCard.svelte";
	import ScrollDetector from "$lib/components/ScrollDetector.svelte";
	import { onMount } from "svelte";
	import { page } from "$app/state";
	import { goto } from "$app/navigation";
	import ListPage from "$lib/components/page-components/ListPage.svelte";

	const DEFAULT_SEARCH = ""
	const DEFAULT_SORT = 1
	const DEFAULT_ORDER = true

	let loading = $state(false);
	let creatorList = $state<CreatorListData[]>([]);
	let currentPage = $state(0);
	let sortDesc = $state(DEFAULT_ORDER);
	let sortSelection = $state(DEFAULT_SORT)
	let searchQuery = $state(DEFAULT_SEARCH);

	const sortOptions = [
		{id: "mod-count",    path: mdiPackageVariant, title: "Mod Count"},
		{id: "download",     path: mdiDownload,     title: "Downloads"},
		{id: "favorites",    path: mdiStar,         title: "Favorites"},
		{id: "views",        path: mdiEye,          title: "Views"}
	]

	const fetchCreators = async (): Promise<CreatorListData[]> => {
		return await ((await fetch(`/api/creator_list/${currentPage}?desc=${sortDesc}&sort=${sortSelection}&search=${searchQuery}`)).json());
	};

	onMount(() => {
		const searchParams = page.url.searchParams
		searchQuery = searchParams.get("search") ?? DEFAULT_SEARCH
		sortSelection = Number.parseInt(searchParams.get("sort") ?? DEFAULT_SORT.toString())
		sortDesc = (searchParams.get("desc") ?? DEFAULT_ORDER.toString()) == "true"
	})

	async function updateCreatorList() {

		const params = new URLSearchParams()
		if (searchQuery != DEFAULT_SEARCH) params.set("search", searchQuery)
		if (sortSelection != DEFAULT_SORT) params.set("sort", sortSelection.toString())
		if (sortDesc != DEFAULT_ORDER) params.set("desc", "false")
		goto(`?${params.toString()}`)

		currentPage = 0
		loading = true
		creatorList = await fetchCreators()
		currentPage++
		loading = false
	}

	async function onScrollDetect() {
		const creators = await fetchCreators();
		creatorList.push(...creators);
		currentPage++;
	}

	function resetParams() {
		searchQuery = DEFAULT_SEARCH
		sortSelection = DEFAULT_SORT
		sortDesc = DEFAULT_ORDER
		updateCreatorList()
	}
</script>

<svelte:head>
	<title>tMLDB - Creators</title>
	<meta name="description" content="View a list of all Terraria modders">
</svelte:head>

<ListPage onsubmit={updateCreatorList} resetControls={resetParams}>
	{#snippet controls()}
		<SortSelector 
			sortOptions={sortOptions}
			bind:selectedIdx={sortSelection} 
			bind:sortDesc={sortDesc} 
			onchange={updateCreatorList} 
		/>
	{/snippet}

	{#snippet list()}
		<div id="creators-list">
			{#each creatorList as creator}
				<CreatorListCard {creator} />
			{/each}
			<ScrollDetector bind:loading onDetect={onScrollDetect} />
		</div>
	{/snippet}
</ListPage>

<style>
	#creators-list {
		--min-card-size: 19rem;
		display: grid;
		grid-template-columns: repeat(auto-fit, minmax(var(--min-card-size), 1fr));
		overflow-y: auto;
		gap: 1rem;
		padding-right: 0.5rem;


		@media (max-width: 375px) {
			--min-card-size: 15rem;
		}
	}
</style>