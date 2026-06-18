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
	<p>Sort by</p>
	<button
		class="dropdown-toggle"
		popovertarget="sort-dropdown"
		title={"Sort by " + sortOptions[selectedIdx].title}
	>
		<Icon path={sortOptions[selectedIdx].path} />
		<Icon path={mdiMenuDown} size="20" />
	</button>

	<div id="sort-dropdown" class="panel" popover>
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

	<button title={sortOrderTitle} onclick={() => {sortDesc = !sortDesc; onchange?.()} }>
		{#if sortDesc}
			<Icon path={mdiSortAscending} />
		{:else}
			<Icon path={mdiSortDescending} />
		{/if}
	</button>
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
		padding: 5px 10px;
	}

	#sort-dropdown {
		position: absolute;
		position-anchor: --sort-dropdown-anchor;
		top: calc(anchor(bottom) + 0.25rem);
		left: anchor(left);

		flex-direction: column;
		gap: 0.25rem;

		padding: 0.5rem;
		background-color: var(--panel-bg);
		overflow: visible; /* fixes .panel scrollbar issue */
		backdrop-filter: brightness(0.75) blur(10px);

		&:popover-open {
			display: flex;
		}
	}

	.dropdown-item {
		display: flex;
		align-items: center;
		gap: 0.5rem;
		padding: 8px 12px;

		&.selected {
			--panel-border: var(--yellow);
			background-color: var(--panel-col2);
		}
	}

	button {
		display: flex;
		padding: 5px 10px;
	}
</style>
