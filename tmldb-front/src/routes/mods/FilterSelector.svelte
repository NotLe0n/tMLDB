<script lang="ts">
	import { mdiMenuDown } from "@mdi/js";
	import Icon from "$lib/components/Icon.svelte";

	let { modSides = $bindable([]), tags = $bindable([]), versions = $bindable([]), onapply } = $props()

	type ModFiltersData = {
		mod_sides: string[],
		tags: string[],
		tml_versions: string[]
	}

	const fetchFilters = async (): Promise<ModFiltersData> => {
		return await ((await fetch("/api/mod_filters")).json())
	}

	let filtersPromise = $state(fetchFilters())

	function getSelectedVersionsStr(versions: string[]) {
		if (versions.length == 0) {
			return 'Select versions'
		}
		if (versions.length < 3) {
			return versions.join(", ")
		}
		return versions.slice(0, 2).join(", ") + " + " + (versions.length - 2)
	}
	let selectedVersionsStr = $derived(getSelectedVersionsStr(versions))

	function resetSelection() {
		modSides = []
		tags = []
		versions = []
	}

	const isSomethingSelected = $derived(modSides.length > 0 || tags.length > 0 || versions.length > 0)
</script>

<div id="filter-selector-container">
	<p>Filter by</p>
	<button class="dropdown-toggle" popovertarget="filter-dropdown">
		<Icon path={mdiMenuDown} size="20" />
	</button>
	
	<div id="filter-dropdown" popover>
		{#await filtersPromise}
			<p>Loading Filters</p>
		{:then filters}
			<div>
				<b>Mod Side</b>
				<div class="filter-container">
					{#each filters.mod_sides as mod_side, i}
						<input 
							type="checkbox" 
							id="mod-side-filter-{i}"
							bind:group={modSides}
							value={mod_side}
						>
						<label for="mod-side-filter-{i}" title={mod_side}>{mod_side}</label>
					{/each}
				</div>
			</div>
			<div>
				<b>Tag</b>
				<div class="filter-container">
					{#each filters.tags as tag, i}
						<input
							type="checkbox"
							id="tag-filter-{i}"
							bind:group={tags}
							value={tag}
						>
						<label for="tag-filter-{i}" title={tag}>{tag}</label>
					{/each}
				</div>
			</div>
			<div id="version-filter-section">
				<b>tModloader Version</b>
				<button class="version-dropdown-toggle" popovertarget="version-dropdown">
					{selectedVersionsStr}
					<Icon path={mdiMenuDown} size="16" />
				</button>
				
				<div id="version-dropdown" popover>
					<div class="filter-container">
						{#each filters.tml_versions as version, i}
							<input
								type="checkbox"
								id="version-filter-{i}"
								bind:group={versions}
								value={version}
							>
							<label for="version-filter-{i}" title={version}>{version}</label>
						{/each}
					</div>
				</div>
			</div>
			<div id="filter-buttons">
				<button onclick={onapply}>Apply Filters</button>
				<button disabled={!isSomethingSelected} onclick={() => resetSelection()}>Reset Filters</button>
			</div>
		{/await}
	</div>

</div>

<style>
	#filter-selector-container {
		display: flex;
		gap: 0.5rem;
		align-items: center;
		anchor-name: --filter-dropdown-anchor;
	}

	button {
		display: flex;
		align-items: center;
		gap: 0.25rem;
		background-color: var(--tertiary-bg);
		border: 1px solid var(--secondary);
		border-radius: .5rem;
		padding: 7px 10px;
		cursor: pointer;
		transition: border-color .2s, background-color .2s, opacity .5s;

		&:hover {
			background-color: var(--tertiary-hov);
		}

		&:disabled {
			opacity: 0.5;
			cursor: not-allowed;
		}
	}

	button:disabled:hover {
		background-color: var(--tertiary-bg);
	}

	#filter-dropdown {
		max-width: 35rem;

		position: absolute;
		position-anchor: --filter-dropdown-anchor;
		top: calc(anchor(bottom) + 0.25rem);
		right: anchor(right);
		left: auto;

		flex-direction: column;
		gap: 1rem;

		border: 1px solid var(--secondary);
		border-radius: .5rem;
		padding: 0.5rem;
		background-color: var(--tertiary-bg);
		color: white;
		backdrop-filter: brightness(0.75) blur(10px);

		&:popover-open {
			display: flex;
		}
	}

	input[type="checkbox"] {
		display: none;
	}

	label {
		padding: 0.25rem 0.5rem;
		background-color: var(--tertiary-bg);
		border: 1px solid var(--primary);
		border-radius: 0.75rem;

		&:hover {
			background-color: var(--tertiary-hov);
		}
	}

	input[type="checkbox"]:checked + label {
		border-color: var(--highlight);
	}

	.filter-container {
		display: flex;
		flex-wrap: wrap;
		gap: 0.5rem;
		padding: .5rem;
		overflow: auto;
	}

	#version-filter-section {
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
		anchor-name: --version-dropdown-anchor;
	}

	.version-dropdown-toggle {
		padding: 0.5rem;
		font-size: 0.9rem;
		justify-content: space-between;
		width: 100%;
	}

	#version-dropdown {
		max-width: 30rem;
		max-height: 20rem;
		overflow-y: auto;

		position: absolute;
		position-anchor: --version-dropdown-anchor;
		top: calc(anchor(bottom) + 0.25rem);
		left: anchor(left);

		border: 1px solid var(--secondary);
		border-radius: .5rem;
		padding: 0.5rem;
		background-color: var(--tertiary-bg);
		color: white;
		backdrop-filter: brightness(0.75) blur(10px);

		&:popover-open {
			display: block;
		}
	}

	#filter-buttons {
		display: grid;
		grid-template-columns: 1fr 1fr;
		gap: 1rem;
	}
</style>