<script lang="ts">
	type SearchFilterProps = { searchQuery: string, onsubmit: any }
	let { searchQuery = $bindable(), onsubmit } : SearchFilterProps = $props()
</script>

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

<style>
	form {
		display: grid;
		grid-template-columns: 1fr min-content;
		gap: 0.5rem;
		flex: 1;
	}

	.search-bar {
		min-width: 25ch;
		outline: none;
		background-color: var(--tertiary-bg);
		border: 1px solid var(--secondary);
		border-radius: 0.5rem;
		padding: 0.5rem 1rem;
		transition: border-color 0.3s, box-shadow 0.3s;

		&:focus {
			box-shadow: 0 0 2px 3px var(--tertiary-hov);
			border-color: var(--highlight);
		}

		@media (max-width: 700px) {
			min-width: 15ch;
		}
	}

	.search-button {
		background-color: var(--tertiary-bg);
		border: 1px solid var(--secondary);
		border-radius: 0.5rem;
		padding: 0.5rem 1rem;
		transition: border-color .2s, background-color .2s;
		cursor: pointer;

		&:hover {
			background-color: var(--tertiary-hov);
		}
	}
</style>