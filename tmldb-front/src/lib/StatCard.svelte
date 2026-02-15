<script lang="ts">
    import Icon from './Icon.svelte';

	let { label, value, icon } = $props()

	const formattedValue = 
		$derived(Number.isInteger(value)
			? new Intl.NumberFormat('en-US', { notation: 'compact' }).format(value)
			: value
		)
</script>

<div class="stat-card" title="{label}: {formattedValue}">
	<span class="stat-icon"><Icon path={icon} color="var(--secondary)" /></span>
	<div class="stat-content">
		<span class="stat-label">{label}</span>
		<strong class="stat-value">{formattedValue}</strong>
	</div>
</div>

<style>
	.stat-card {
		display: flex;
		min-width: 10rem;
		gap: 0.75rem;
		padding: 1rem;
		background: rgba(255, 255, 255, 0.08);
		border: 1px solid rgba(255, 255, 255, 0.12);
		border-radius: 0.85rem;
		backdrop-filter: blur(10px);
		flex: 1;
	}

	.stat-icon {
		font-size: 1.5rem;
		display: flex;
		align-items: center;
		justify-content: center;
		width: 2.5rem;
		height: 2.5rem;
		background: rgba(255, 255, 255, 0.06);
		border-radius: 0.5rem;
	}

	.stat-content {
		display: flex;
		flex-direction: column;
		gap: 0.2rem;
		justify-content: center;
	}

	.stat-label {
		font-size: 0.7rem;
		text-transform: uppercase;
		letter-spacing: 0.08em;
		color: rgba(255, 255, 255, 0.65);
	}

	.stat-value {
		font-size: 1.35rem;
		color: rgba(255, 255, 255, 0.95);
		font-weight: 700;
	}

	@media (max-width: 480px) {
		.stat-card {
			min-width: 7rem;
		}

		.stat-label {
			display: none;
		}
	}
</style>