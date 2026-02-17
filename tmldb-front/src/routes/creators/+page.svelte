<script lang="ts">
	import { type CreatorListData } from "$lib"
	import { mdiDownload, mdiStar, mdiEye, mdiPackageVariant, mdiCogRefresh } from "@mdi/js";
	import SortSelector from "$lib/SortSelector.svelte";
	import CreatorListCard from "./CreatorListCard.svelte";
    import SearchFilter from "$lib/SearchFilter.svelte";
    import ScrollDetector from "$lib/ScrollDetector.svelte";
    import { onMount } from "svelte";
    import { page } from "$app/state";
    import { goto } from "$app/navigation";
    import Icon from "$lib/Icon.svelte";

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

<div id="creators-container">
	<header>
		<h2>Creators</h2>
		<div id="controls">
			<SearchFilter bind:searchQuery onsubmit={updateCreatorList} />
			<div class="controls-box">
				<SortSelector 
					sortOptions={sortOptions}
					bind:selectedIdx={sortSelection} 
					bind:sortDesc={sortDesc} 
					onchange={updateCreatorList} 
				/>
				<button id="reset-button" title="Reset all" onclick={resetParams}>
					<Icon path={mdiCogRefresh} />
				</button>
			</div>
		</div>
	</header>
	<div id="creators-list">
		{#each creatorList as creator}
			<CreatorListCard {creator} />
		{/each}
		<ScrollDetector bind:loading onDetect={onScrollDetect} />
	</div>
</div>

<style>
#creators-container {
	display: grid;
	height: calc(100vh - var(--nav-height) - var(--footer-height) - 1rem);
	grid-template-rows: auto 1fr;
	gap: .5rem;
	padding-top: 1rem;
}

header {
	padding: 0 3.5rem;
	h2 {
		text-align: center;
		margin-bottom: 20px;
	}
}

#controls {
	display: flex;
	flex-wrap: wrap;
	gap: 0.5rem 1rem;
	align-items: center;
	justify-content: center;
	padding: 0 10px;
}

.controls-box {
	display: flex;
	flex-wrap: wrap;
	align-items: center;
	justify-content: center;
	gap: 0.25rem 1rem;
}

#reset-button {
	display: flex;
	background-color: var(--tertiary-bg);
	border: 1px solid var(--secondary);
	border-radius: .5rem;
	padding: 5px 10px;
	cursor: pointer;
	transition: border-color .2s, background-color .2s;

	&:hover {
		background-color: var(--tertiary-hov);
	}
}

#creators-list {
	--min-card-size: 19rem;
	display: grid;
	grid-template-columns: repeat(auto-fit, minmax(var(--min-card-size), 1fr));
	overflow-y: auto;
	gap: 1rem;

	--scrollbar-area: 10px;
	--space-area: 3.5rem;
	padding-left: calc(var(--space-area) + var(--scrollbar-area));
	padding-right: var(--scrollbar-area);
	margin-right: calc(var(--space-area) - var(--scrollbar-area));

	@media (max-width: 520px) {
		--space-area: 2rem;
	}

	@media (max-width: 375px) {
		--min-card-size: 15rem;
	}
}
</style>
