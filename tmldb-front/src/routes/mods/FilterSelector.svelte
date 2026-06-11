<script lang="ts">
	import { mdiMenuDown } from "@mdi/js";
	import Icon from "$lib/components/Icon.svelte";

	type FilterSelectorProps = { modSides: string[], tags: string[], versions: string[], onapply: any}
	let { modSides = $bindable([]), tags = $bindable([]), versions = $bindable([]), onapply }: FilterSelectorProps = $props()

	type ModFiltersData = {
		mod_sides: string[],
		tags: string[],
		tml_versions: string[]
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

	let versionDropdown: HTMLDivElement | undefined = $state()
	let versionInputText = $state("")
	let focusTimer = $state<number | null>(null)

	function getVersionMajor(v: string): number {
		return Number.parseInt(v.replace("tModLoader v", "").split(".")[0])
	}

	function normVersion(v: string): string {
		return v.replace("tModLoader v", "")
	}

	function closePopup() {
		if (focusTimer) clearTimeout(focusTimer)
		focusTimer = setTimeout(() => {
			if (!versionDropdown?.contains(document.activeElement)) {
				versionDropdown?.hidePopover()
			}
		}, 50)
	}
</script>

<div id="filter-selector-container">
	<p>Filter by</p>
	<button class="dropdown-toggle" popovertarget="filter-dropdown">
		<Icon path={mdiMenuDown} size="20" />
	</button>

	<div id="filter-dropdown" class="panel" popover>
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
						<label for="mod-side-filter-{i}" title={mod_side} class="tag panel-small hover-highlight">{mod_side}</label>
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
						<label for="tag-filter-{i}" title={tag} class="tag panel-small hover-highlight">{tag}</label>
					{/each}
				</div>
			</div>
			<div>
				<b>tModloader Version</b>
				<div id="version-filter-section">
					<div id="version-select-header">
						<p>Selected versions:</p>
						<div class="panel-small--wrapper">
							<input type="text" placeholder="enter version"
								id="version-input"
								bind:value={versionInputText}
								onfocus={() => versionDropdown?.showPopover() }
								onfocusout={() => closePopup() }
							>
						</div>
					</div>
					<div class="selected-versions">
						{#each versions as version}
							<button class="tag" onclick={() => versions = versions.filter(v => v != version)}>
								{normVersion(version)}
							</button>
						{:else}
							<span>No versions selected</span>
						{/each}
					</div>
				</div>
			</div>

			<div id="version-dropdown" class="panel" popover="manual" bind:this={versionDropdown} onfocusout={() => closePopup()}>
				<div class="filter-container">
					{#each filters.tml_versions
						.filter(v => v.startsWith(normVersion(versionInputText)))
						.filter(v => !versions.includes(v))
						.sort((a, b) => getVersionMajor(b) - getVersionMajor(a))
					as version, i}
						<button title="{version}" class="tag" onclick={() => versions.push(version)}>
							{normVersion(version)}
						</button>
					{/each}
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
		overflow: visible; /* fixes .panel scrollbar issue */

		position: absolute;
		position-anchor: --filter-dropdown-anchor;
		top: calc(anchor(bottom) + 0.25rem);
		right: anchor(right);
		left: auto;

		flex-direction: column;
		gap: 1rem;

		padding: 0.5rem;
		background-color: var(--panel-bg);
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
		background-color: var(--panel-col2);
	}

	label.tag[for] {
		cursor: pointer;
	}

	input[type="checkbox"]:checked + label.tag {
		--panel-border: var(--yellow);
	}

	.filter-container {
		display: flex;
		flex-wrap: wrap;
		gap: 0.5rem;
		padding: .5rem;
		overflow: auto;
	}

	#version-select-header {
		display: flex;
		justify-content: space-between;
		align-items: center;
	}

	#version-filter-section {
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
		padding: 0 .5rem;
	}

	#version-input {
		padding: 0.5rem;
		width: 30ch;
		anchor-name: --version-dropdown-anchor;
	}

	.selected-versions {
		display: flex;
		align-items: center;
		gap: 0.5rem;
		overflow-x: auto;
		overflow-y: hidden;
	}

	#version-dropdown {
		overflow: visible;

		position: absolute;
		position-anchor: --version-dropdown-anchor;
		top: calc(anchor(bottom) + 0.25rem);
		left: anchor(left);

		padding: 0.5rem;
		color: white;

		&:popover-open {
			display: block;
		}

		& > .filter-container {
			max-height: 20rem;
			max-width: 20rem;
		}
	}

	#filter-buttons {
		display: grid;
		grid-template-columns: 1fr 1fr;
		gap: 1rem;
	}
</style>
