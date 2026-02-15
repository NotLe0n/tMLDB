<script lang="ts">
	type ModTag = {
		tag: string;
		display_name: string;
	}

	const sides = ["Both", "Client", "Server", "NoSync"]
	const { tag }: { tag: ModTag } = $props()
</script>

{#if sides.includes(tag.tag)}
	<span title={tag.tag} class="mod-side">Mod Side: {tag.display_name}</span>
{:else}
	<span title={tag.tag}>{tag.display_name}</span>
{/if}

<style>
	span {
		padding: 0.25rem 0.75rem;
		border-radius: 1rem;
		font-size: 0.75rem;
		background: rgba(255, 255, 255, 0.12);
		border: 1px solid rgba(255, 255, 255, 0.2);
		order: 1;
	}

	.mod-side {
		order: 0;
	}
</style>