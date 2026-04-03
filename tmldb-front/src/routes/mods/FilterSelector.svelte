<script lang="ts">
	import { mdiMenuDown } from "@mdi/js";
	import Icon from "$lib/components/Icon.svelte";

	type FilterSelectorProps = { modSides: string[], tags: string[], versions: string[], onapply: any}
	let { modSides = $bindable([]), tags = $bindable([]), versions = $bindable([]), onapply }: FilterSelectorProps = $props()

	type ModFiltersData = {
		mod_sides: string[],
		tags: string[],
		mod_versions: string[]
	}

	const fetchFilters = async (): Promise<ModFiltersData> => {
		return await ((await fetch("/api/mod_filters")).json())
	}

	let filtersPromise = $state(fetchFilters())

	function resetSelection() {
		modSides = []
		tags = []
		versions = []
	}

	const isSomethingSelected = $derived(modSides.length > 0 || tags.length > 0 || versions.length > 0)

	function getVersionMajor(v: string): number {
		return Number.parseInt(v.replace("tModLoader v", "").split(".")[0])
	}
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
						<label for="mod-side-filter-{i}" title={mod_side} class="tag">{mod_side}</label>
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
						<label for="tag-filter-{i}" title={tag} class="tag">{tag}</label>
					{/each}
				</div>
			</div>
			<div id="version-filter-section">
				<b>tModloader Version</b>
				<button class="version-dropdown-toggle" popovertarget="version-dropdown">
					<div class="selected-versions">
						{#each versions.slice(0, 4) as version}
							<div class="tag">{version.replace("tModLoader v", "")}</div>
						{:else}
							<span>Select versions</span>
						{/each}
						{#if versions.length > 4}
							<span>+ {versions.length - 4}</span>
						{/if}
					</div>
					
					<Icon path={mdiMenuDown} size="16" />
				</button>
				
				<div id="version-dropdown" popover>
					<div class="filter-container">
						{#each filters.mod_versions.sort((a, b) => getVersionMajor(b) - getVersionMajor(a)) as version, i}
							<input
								type="checkbox"
								id="version-filter-{i}"
								bind:group={versions}
								value={version}
							>
							<label for="version-filter-{i}" title={version} class="tag">{version.replace("tModLoader v", "")}</label>
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
		padding: 6px 10px;
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

		border: 1px solid var(--green2);
		border-radius: 0.5rem;
		padding: 0.5rem;
		background-color: var(--green3-bg);
		color: white;
		backdrop-filter: brightness(0.75) blur(10px);

		&:popover-open {
			display: flex;
		}
	}

	input[type="checkbox"] {
		display: none;
	}

	.tag {
		padding: 0.25rem 0.5rem;
		background-color: rgba(255, 255, 255, 0.12);
		border: 1px solid rgba(255, 255, 255, 0.2);
		border-radius: 0.25rem;
	}

	label.tag[for] {
		cursor: pointer;

		&:hover {
			background-color: rgba(255, 255, 255, 0.2);
			border-color: rgba(255, 255, 255, 0.25);
		}
	}

	input[type="checkbox"]:checked + label.tag {
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

	.selected-versions {
		display: flex;
		align-items: center;
		gap: 0.5rem;
	}

	#version-dropdown {
		max-width: 30rem;
		max-height: 20rem;
		overflow-y: auto;

		position: absolute;
		position-anchor: --version-dropdown-anchor;
		top: calc(anchor(bottom) + 0.25rem);
		left: anchor(left);

		border: 1px solid var(--green2);
		border-radius: .5rem;
		padding: 0.5rem;
		background-color: var(--green3-bg);
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