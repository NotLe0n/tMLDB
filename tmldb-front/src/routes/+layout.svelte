<script lang="ts">
	import favicon from '$lib/assets/favicon.svg';
	import Icon from '$lib/components/Icon.svelte';
	import { mdiAccount, mdiPackageVariant } from '@mdi/js';
	import ModSearch from '$lib/ModSearch.svelte';
	import type { SearchResult } from '$lib';
	import { page } from '$app/state';

	let { children } = $props()

	let searchMod = $state(true)
</script>

<svelte:head>
	<link rel="icon" href={favicon} />
</svelte:head>

<header>
	<div class="nav-left">
		<span class="logo">tMLDB</span>
		<nav>
			<span><a href="/" class:current-site={page.url.pathname==="/"}>Home</a></span>
			<span><a href="/mods" class:current-site={page.url.pathname==="/mods"}>Mods</a></span>
			<span><a href="/creators" class:current-site={page.url.pathname==="/creators"}>Creators</a></span>
			<span><a href="/compare" class:current-site={page.url.pathname==="/compare"}>Compare</a></span>
		</nav>
	</div>
	<form id="search-form">
		<input id="search-type" type="checkbox" bind:checked={searchMod} >
		<label for="search-type" class="panel-small" title="Click to toggle mods/creators">
			{#if searchMod}
				<Icon path={mdiPackageVariant} />
			{:else}
				<Icon path={mdiAccount} />
			{/if}
		</label>

		<ModSearch {searchMod}>
			{#snippet resultContent(searchResults: SearchResult[])}
				{#each searchResults as res}
					<a href="/{searchMod ? "mod" : "creator"}/{res.id}">{res.name}</a>
				{/each}
			{/snippet}
		</ModSearch>
	</form>
</header>

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
	header {
		box-sizing: border-box;
		position: fixed;
		width: 100%;
		height: var(--nav-height);
		top: 0;
		background: #234c8169;
		backdrop-filter: blur(10px);
		border-bottom: 1px solid var(--divider2);
		z-index: 1;

		display: flex;
		padding: 0 2rem;
		align-items: center;
		justify-content: space-between;
		gap: 1rem 4rem;

		@media (max-width: 768px) {
			justify-content: center;
			flex-direction: column;

			.nav-left {
				gap: 1rem;
			}
			nav > span {
				padding: 0.5rem;
			}
		}
	}

	.nav-left {
		display: flex;
		align-items: center;
		gap: 2rem;
	}

	.logo {
		background: var(--gradient);
		background-clip: text;
		color: transparent;
		font-weight: 700;
		font-size: 1.5rem;
		text-decoration: none;
		transition: transform 0.2s;
	}

	nav {
		display: flex;
	}

	nav > span {
		padding: 0.5rem 1rem;
		&:hover > a {
			anchor-name: --hover-anchor;
		}
	}

	nav:not(:has(span:hover)) a.current-site {
		anchor-name: --hover-anchor;
	}

	nav > span > a {
		color: rgb(from white r g b / 0.8);
		text-decoration: none;
		font-size: 0.95rem;
		font-weight: 500;
		transition: border-color 0.3s;
		padding: 0.5rem 0;
		margin-top: 2px;

		&:hover {
			color: var(--yellow);
		}
	}

	nav::after {
		position: absolute;
		content: '';
		height: 2px;
		position-anchor: --hover-anchor;
		left: anchor(left);
		bottom: anchor(bottom);
		right: anchor(right);
		transition: inset 200ms;
		background-color: var(--yellow);
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
		width: 2rem;
		cursor: pointer;

		&:hover {
			--panel-border: var(--yellow);
		}
	}

	main {
		padding-top: var(--nav-height);
		min-height: calc(100vh - var(--nav-height) - var(--footer-height) - 2rem);
	}

	footer {
		display: var(--footer-display);
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
