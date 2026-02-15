<script lang="ts">
	import { mdiMenuDown, mdiSortAscending, mdiSortDescending } from "@mdi/js";
    import Icon from "./Icon.svelte";

	type SortOptions = {
		id: string;
		path: string;
		title: string;
	}

	type SortSelectorProps = {
		sortOptions: SortOptions[],
		selectedIdx: number,
		sortDesc: boolean,
		onchange: any
	}

	let { sortOptions, selectedIdx = $bindable(), sortDesc = $bindable(), onchange }: SortSelectorProps = $props()
	const sortOrderTitle = $derived(sortDesc ? "Order: Descending" : "Order: Ascending")
	
	function selectOption(index: number) {
		selectedIdx = index
		onchange?.()
	}
</script>

<div id="sort-selector-container">
	<button 
		class="dropdown-toggle" 
		popovertarget="sort-dropdown"
		title={"Sort by " + sortOptions[selectedIdx].title}
	>
		<Icon path={sortOptions[selectedIdx].path} />
		<Icon path={mdiMenuDown} size="20" />
	</button>
	
	<div id="sort-dropdown" popover>
		{#each sortOptions as option, i}
			<button
				class="dropdown-item"
				class:selected={i === selectedIdx}
				onclick={() => selectOption(i)}
				title={"Sort by " + option.title}
				popovertarget="sort-dropdown"
				popovertargetaction="hide"
			>
				<Icon path={option.path} />
				<span>{option.title}</span>
			</button>
		{/each}
	</div>

	<input type="checkbox" id="sort-order" name="sort-order" bind:checked={sortDesc} {onchange}>
	<label for="sort-order" title={sortOrderTitle}>
		{#if sortDesc}
			<Icon path={mdiSortAscending} />
		{:else}
			<Icon path={mdiSortDescending} />
		{/if}
	</label>
</div>

<style>
	#sort-selector-container {
		display: flex;
		gap: 0.5rem;
		align-items: center;
		anchor-name: --sort-dropdown-anchor;
	}

	.dropdown-toggle {
		display: flex;
		align-items: center;
		gap: 0.25rem;
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

	#sort-dropdown {
		position: absolute;
		position-anchor: --sort-dropdown-anchor;
		top: calc(anchor(bottom) + 0.25rem);
		left: anchor(left);

		flex-direction: column;
		gap: 0.25rem;

		border: 1px solid var(--secondary);
		border-radius: .5rem;
		padding: 0.25rem;
		background-color: var(--tertiary-bg);
		backdrop-filter: brightness(0.75) blur(10px);

		&:popover-open {
			display: flex;
		}
	}

	.dropdown-item {
		display: flex;
		align-items: center;
		gap: 0.5rem;
		background-color: transparent;
		border: 1px solid transparent;
		border-radius: .25rem;
		padding: 8px 12px;
		cursor: pointer;
		transition: border-color .2s, background-color .2s;

		&:hover {
			background-color: var(--tertiary-hov);
		}

		&.selected {
			background-color: var(--tertiary-hov);
			border-color: var(--highlight);
		}
	}

	input[type='checkbox'] {
		display: none;
	}

	label {
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
</style>