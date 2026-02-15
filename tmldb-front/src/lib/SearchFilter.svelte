<script lang="ts">
	type SearchFilterProps = { searchQuery: string, onsubmit: any }
	let { searchQuery = $bindable(), onsubmit } : SearchFilterProps = $props()
</script>

<form {onsubmit} method="GET">
	<input
		type="text"
		name="search"
		placeholder="Filter by name..." 
		bind:value={searchQuery}
		class="search-bar"
	/>
	<button type="submit" class="search-button" >Filter</button>
</form>

<style>
	form {
		display: flex;
		gap: 0.5rem;
		flex: 1;
	}

	.search-bar {
		flex: 1;
		padding: 0.75rem 1rem;

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