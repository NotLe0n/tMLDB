<script lang="ts">
	import type { SearchResult } from "$lib";
	import { afterNavigate } from "$app/navigation";

	let { searchMod, resultContent, onSubmit }: { searchMod: boolean, resultContent: any, onSubmit: any } = $props()
	let searchQuery = $state("")
	let searchResults = $state<SearchResult[]>([])
	let searchTimer = $state<number | null>(null)
	let focusTimer = $state<number | null>(null)
	let resultContainer: HTMLDivElement | null
	const uid = $props.id()

	function onKeySearch() {
		if (searchTimer) clearTimeout(searchTimer)
		searchTimer = setTimeout(search, 200)
	}

	function onEnter(evt: KeyboardEvent) {
		if (evt.key === "Enter") {
			onSubmit?.(searchResults)
			if (searchTimer) clearTimeout(searchTimer)
			return
		}
	}

	async function search() {
		let searchUrl;
		if (searchMod) {
			searchUrl = `/api/search_mod?query=${searchQuery}`
		}
		else {
			searchUrl = `/api/search_creator?query=${searchQuery}`
		}

		searchResults = await (await fetch(searchUrl)).json()
		if (searchResults.length > 0) {
			resultContainer?.showPopover()
		}
		else {
			resultContainer?.hidePopover()
		}
	}

	function openResultContainer() {
		if (searchResults.length > 0) {
			resultContainer?.showPopover()
		}
	}

	function closeResultContainer() {
		if (focusTimer) clearTimeout(focusTimer)
		focusTimer = setTimeout(() => {
			if (!resultContainer?.contains(document.activeElement)) {
				resultContainer?.hidePopover()
			}
		}, 50)
	}

	afterNavigate(() => {
		searchQuery = ""
		searchResults = []
		resultContainer?.hidePopover()
	})
</script>

<div class="panel-small--wrapper">
	<input
		class="search-bar panel-small"
		type="text"
		bind:value={searchQuery}
		style="anchor-name: --mod-search-{uid};"
		oninput={onKeySearch}
		onkeydown={onEnter}
		onfocusin={() => openResultContainer()}
		onfocusout={() => closeResultContainer()}
		placeholder="Search {searchMod ? 'mods' : 'creators'}..."
	>
</div>
<div class="search-result-container panel"
	popover="manual"
	bind:this={resultContainer}
	style="position-anchor: --mod-search-{uid};"
	onfocusout={() => closeResultContainer()}>
	<div class="search-results-scroll">
		{@render resultContent(searchResults)}
	</div>
</div>

<style>
	.panel-small--wrapper {
		width: auto;
		display: grid;
	}

	.search-bar {
		appearance: none;
		outline: none;
		padding: 0.5rem 1rem;
	}

	.search-result-container {
		position: fixed;
		top: calc(anchor(bottom) + 0.25rem);
		left: anchor(left);

		width: 290px;
		padding: .25rem;
		background-color: var(--panel-bg);
		backdrop-filter: blur(10px);
		overflow: visible;
		transition:
			opacity 0.3s ease,
			display 0.3s ease allow-discrete,
			overlay 0.3s ease allow-discrete;
		opacity: 0;

		&:popover-open {
			display: block;
			opacity: 1;

			@starting-style {
				& {
					opacity: 0;
				}
			}
		}
	}

	.search-results-scroll {
		display: flex;
		flex-direction: column;
		max-height: 40rem;
		overflow-y: auto;
	}

	:global(.search-result-container > .search-results-scroll > *) {
		text-align: start;
		border-radius: .5rem;
		outline: none;
		border: 1px solid transparent;
		padding: .55rem;

		&:hover {
			color: var(--yellow);
			background-color: var(--panel-col2);
		}

		&:focus {
			color: var(--yellow);
			background-color: var(--panel-col2);
			border: 1px solid var(--yellow);
		}
	}
</style>
