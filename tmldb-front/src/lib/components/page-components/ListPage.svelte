
<script lang="ts">
	import Icon from "$lib/components/Icon.svelte";
	import { mdiCog, mdiCogRefresh } from "@mdi/js";

	let { list, controls, searchQuery = $bindable(), onsubmit, resetControls } = $props();

</script>

<svelte:head>
	<style>
		:root {
			--footer-display: none;
		}
	</style>
</svelte:head>

<div id="list-container">
	<header>
		<div id="controls">
			<form {onsubmit}>
				<input
					type="text"
					name="search"
					placeholder="Filter by name..." 
					bind:value={searchQuery}
					class="search-bar"
				/>
				<button type="submit" class="search-button">Filter</button>
			</form>
			<label for="controls-button" title="more options"><Icon path={mdiCog} /></label>
			<input type="checkbox" id="controls-button" />
			<div class="controls-box">
				{@render controls()}
				<button id="reset-button" title="Reset all" onclick={resetControls}>
					<Icon path={mdiCogRefresh} />
				</button>
			</div>
		</div>
	</header>
	{@render list()}
</div>

<style>
	#list-container {
		display: grid;
		height: calc(100vh - var(--nav-height) - 1rem);
		grid-template-rows: auto 1fr;
		gap: .5rem;
		padding: 1rem clamp(1rem, 3vw, 4rem) 0;
	}

	#controls-button {
		display: none;
	}

	label[for="controls-button"] {
		display: flex;
		background-color: var(--green3-bg);
		border: 1px solid var(--green2);
		border-radius: var(--controls-radius);
		padding: 5px 10px;
		cursor: pointer;
		transition: border-color .2s, background-color .2s;

		&:hover {
			background-color: var(--green3-hov);
		}

		&:has(+ input:checked) {
			border-color: var(--highlight);
		}

		@media (min-width: 768px) {
			display: none;
		}
	}

	#controls {
		display: flex;
		flex-wrap: wrap;
		gap: 0.5rem 1rem;
		align-items: center;
		justify-content: center;
	}

	.controls-box {
		display: flex;
		flex-wrap: wrap;
		align-items: center;
		justify-content: center;
		gap: 0.25rem 1rem;
	}

	form {
		display: grid;
		grid-template-columns: 1fr min-content;
		gap: 0.5rem;
		flex: 1;
	}

	.search-bar {
		min-width: 25ch;
		outline: none;
		padding: 0.5rem 1rem;

		@media (max-width: 700px) {
			min-width: 15ch;
		}
	}

	.search-button {
		padding: 0.5rem 1rem;
	}

	#reset-button {
		display: flex;
		padding: 5px 10px;
	}

	@media (max-width: 768px) {
		#controls-button:not(:checked) + .controls-box {
			display: none;
		}
	}
</style>