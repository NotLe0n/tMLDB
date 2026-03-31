<script lang="ts">
    import type { SearchResult } from "$lib";

	let { searchMod, resultContent }: { searchMod: boolean, resultContent: any } = $props()
	let searchQuery = $state("")
	let searchResults = $state<SearchResult[]>([])
	let searchTimer = $state<number | null>(null)
	let resultContainer: HTMLDivElement
	const uid = $props.id()

	function onKeySearch() {
		if (searchTimer) clearTimeout(searchTimer)
		searchTimer = setTimeout(search, 200)
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
			resultContainer.showPopover()
		}
		else {
			resultContainer.hidePopover()
		}
	}
</script>

<input 
	class="search-bar"
	type="text" 
	bind:value={searchQuery}
	style="anchor-name: --mod-search-{uid};"
	oninput={onKeySearch}
	onfocusin={() => searchResults.length > 0 && resultContainer.showPopover()}
	onfocusout={() => setTimeout(() => resultContainer.hidePopover(), 80)}
	placeholder="Search {searchMod ? 'mods' : 'creators'}..."
>
<div class="search-result-container" popover="manual" bind:this={resultContainer} style="position-anchor: --mod-search-{uid};">
	{@render resultContent(searchResults)}
</div>

<style>
	.search-bar {
		appearance: none;
		outline: none;

		font-size: 0.95rem;
		padding: 0.5rem 1rem;

		background-color: var(--green3-bg);
		border: 1px solid var(--green2);
		border-radius: 0.5rem;
		transition: border-color 0.3s ease, box-shadow 0.3s ease, border-radius 0.3s ease;

		&:focus {
			box-shadow: 0 0 3px 3px var(--green3-hov);
			border-color: var(--highlight);
		}

		&::placeholder {
			color: var(--search-bar-color);
		}

		&:has(+ :popover-open) {
			border-radius: 0.5rem 0.5rem 0 0;
		}
	}

	.search-result-container {
		flex-direction: column;

		position: fixed;
		top: anchor(bottom);
		left: anchor(left);

		width: 290px;
		max-height: 40rem;
		padding: .25rem;
		border: solid 1px var(--green1);
		background-color: var(--green3-bg);
		backdrop-filter: blur(10px);
		border-radius: 0 0 0.5rem 0.5rem;
		overflow-y: auto;
		transition: 
			opacity 0.3s ease,
			display 0.3s ease allow-discrete,
			overlay 0.3s ease allow-discrete;
		opacity: 0;

		&:popover-open {
			display: flex;
			opacity: 1;

			@starting-style {
				& {
					opacity: 0;
				}
			}
		}
	}

	:global(.search-result-container > *) {
		background-color: transparent;
		text-align: start;
		border-radius: .25rem;
		outline: none;
		padding: .55rem;
		border: 1px solid transparent;

		&:hover {
			background-color: var(--green3-hov);
		}

		&:last-child {
			border-radius: .25rem .25rem .75rem .75rem;
		}

		&:focus {
			background-color: var(--green3-hov);
			border-color: var(--green1);
		}
	}
</style>