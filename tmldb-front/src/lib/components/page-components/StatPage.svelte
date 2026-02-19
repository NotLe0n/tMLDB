<script lang="ts">
	let { children } = $props();
</script>

<div id="stat-page">
	{@render children() }
</div>

<style>
	#stat-page {
		padding: clamp(1rem, 3vw, 4rem);
		display: flex;
		flex-direction: column;
		gap: 2.5rem;

		:global {
			article {
				padding: 1.5rem;
				border-radius: 1rem;
				background: var(--tertiary-bg);
				border: 1px solid rgba(132, 204, 22, 0.15);
			}

			article h3 {
				margin-bottom: 1rem;
			}
		}
	}

	
</style>