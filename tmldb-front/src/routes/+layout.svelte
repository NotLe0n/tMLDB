<script lang="ts">
	import favicon from '$lib/assets/favicon.svg';
	import type { SearchResult } from '$lib';
    import Icon from '$lib/Icon.svelte';
    import { mdiAccount, mdiPackageVariant } from '@mdi/js';

	let { children } = $props()

	let searchQuery = $state("")
	let searchResults = $state<SearchResult[]>([])
	let searchTimer = $state<number | null>(null)
	let searchMod = $state(true)

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
	}
</script>

<svelte:head>
	<link rel="icon" href={favicon} />
</svelte:head>

<header>
	<div class="nav-left">
		<span class="logo">tMLDB</span>
		<nav>
			<a href="/">Home</a>
			<a href="/mods">Mods</a>
			<a href="/creators">Creators</a>
			<a href="/tree" hidden>Tree</a>
		</nav>
	</div>
	<form onsubmit={search} id="search-form">
		<input id="search-type" type="checkbox" bind:checked={searchMod} >
		<label for="search-type" title="Click to toggle mods/creators">
			{#if searchMod}
				<Icon path={mdiPackageVariant} />
			{:else}
				<Icon path={mdiAccount} />
			{/if}
		</label>

		<input 
			id="search-bar"
			type="text" 
			bind:value={searchQuery}
			oninput={onKeySearch}
			placeholder="Search {searchMod ? 'mods' : 'creators'}..."
		>
	</form>
</header>
<div class={["search-result-container", searchResults.length == 0 && "empty"]}>
	{#each searchResults as res}
		<a href="/{searchMod ? "mod" : "creator"}/{res.id}">{res.name}</a>
	{/each}
</div>	

<main>
	{@render children()}
</main>

<footer>
	<div>
		<p>Made by <a href="https://github.com/NotLe0n">NotLe0n</a></p>
		<p id="disclaimer">This site is not affiliated with the tModLoader Team or ReLogic</p>
	</div>
	<div id="footer-links">
		<a href="https://github.com/NotLe0n/tMLDB">Github</a>
		<a href="https://tmlapis.le0n.dev">API</a>
	</div>
</footer>

<style>
	:root {
		--dropdown-display: flex;
		--dropdown-opacity: 1;
		--searchbar-border-radius: 0.5rem 0.5rem 0 0;
		
		&:not(:has(.search-bar:focus)):not(:has(.search-result-container:hover)), &:has(.empty) {
			--dropdown-display: none;
			--dropdown-opacity: 0;
			--searchbar-border-radius: 0.5rem;
		}
	}

	header {
		--nav-bg1: rgba(27, 141, 27, 0.15);
		--nav-bg2: rgba(34, 197, 94, 0.1);

		box-sizing: border-box;
		position: fixed;
		width: 100%;
		height: var(--nav-height);
		top: 0;
		background: linear-gradient(90deg, var(--nav-bg1) 0%, var(--nav-bg2) 50%, var(--nav-bg1) 100%);
		backdrop-filter: blur(10px);
		border-bottom: 1px solid rgba(132, 204, 22, 0.2);
		z-index: 1;
		
		display: flex;
		padding: 0 2rem;
		align-items: center;
		justify-content: space-between;
		gap: 1rem 4rem;
	
		@media (max-width: 768px) {
			justify-content: center;
			flex-direction: column;

			.nav-left, nav {
				gap: 1rem;
			}
		}
	}

	.nav-left {
		display: flex;
		align-items: center;
		gap: 3rem;
	}

	.logo {
		background: var(--gradient);
		-webkit-background-clip: text;
		-webkit-text-fill-color: transparent;
		background-clip: text;
		font-weight: 700;
		font-size: 1.5rem;
		text-decoration: none;
		transition: transform 0.2s;
	}

	nav {
		display: flex;
		gap: 2rem;
	}

	nav a {
		color: rgb(from white r g b / 0.8);
		text-decoration: none;
		font-size: 0.95rem;
		font-weight: 500;
		transition: border-color 0.3s;
		padding: 0.5rem 0;
		border-bottom: 2px solid transparent;
		margin-top: 2px;

		&:hover {
			color: var(--highlight);
			border-bottom-color: var(--highlight);
		}
	}

	#search-form {
		display: grid;
		grid-template-columns: 2rem minmax(200px, 300px);
		gap: .5rem;
	}

	#search-type {
		display: none;
	}

	label[for="search-type"] {
		display: flex;
		align-items: center;
		justify-content: center;
		background-color: var(--tertiary-bg);
		border: 1px solid var(--secondary);
		border-radius: .5rem;
		cursor: pointer;
		transition: border-color .2s, background-color .2s;

		&:hover {
			background-color: var(--tertiary-hov);
		}
	}

	#search-bar {
		appearance: none;
		outline: none;

		anchor-name: --search-input;
		font-size: 0.95rem;
		padding: 0.5rem 1rem;

		background-color: var(--tertiary-bg);
		border: 1px solid var(--secondary);
		border-radius: var(--searchbar-border-radius);
		transition: border-color 0.3s ease, box-shadow 0.3s ease, border-radius 0.3s ease;

		&:focus {
			box-shadow: 0 0 3px 3px var(--tertiary-hov);
			border-color: var(--highlight);
		}

		&::placeholder {
			color: var(--search-bar-color);
		}
	}

	.search-result-container {
		display: var(--dropdown-display);
		flex-direction: column;

		position: fixed;
		position-anchor: --search-input;
		top: anchor(bottom);
		left: anchor(left);

		max-width: 300px;
		max-height: 40rem;
		padding: .25rem;
		border: solid 1px var(--primary);
		background-color: var(--tertiary-bg);
		backdrop-filter: blur(10px);
		border-radius: 0 0 1rem 1rem;
		z-index: 2;
		overflow-y: auto;
		transition: opacity 0.3s ease, display 0.3s ease allow-discrete;
		opacity: var(--dropdown-opacity);

		a {
			border-radius: .25rem;
			padding: .75rem;

			&:hover {
				background-color: var(--tertiary-hov);
			}

			&:last-child {
				border-radius: .25rem .25rem .75rem .75rem;
			}
		}

		@starting-style {
			& {
				opacity: 0;
			}
		}
	}

	main {
		padding-top: var(--nav-height);
		min-height: calc(100vh - var(--nav-height) - var(--footer-height));
	}

	footer {
		display: flex;
		align-items: center;
		gap: 1rem;
		justify-content: space-between;
		padding: 1rem 2rem;
		background-color: #090f1c;
		min-height: var(--footer-height);

		#disclaimer {
			color: rgb(from white r g b / 0.6);
			text-wrap: pretty;
		}

		#footer-links {
			display: flex;
			gap: 1rem;
		}
	}
</style>