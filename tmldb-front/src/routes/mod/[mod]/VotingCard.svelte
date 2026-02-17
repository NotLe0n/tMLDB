<script lang="ts">
	let { votes_up, votes_down, score } = $props();

	const percentage = $derived((score * 100).toFixed(2));
	const formatNumber = (value: number) =>
		new Intl.NumberFormat('en-US', { notation: 'compact' }).format(value);
</script>

<div class="voting-card">
	<div class="stat-content">
		<span class="stat-label" title="Steam's rating may differ from the upvote/downvote ratio">Steam Rating</span>
		<strong class="stat-value">{percentage}%</strong>
	</div>

	<div class="stat-content">
		<span class="stat-label">Upvotes</span>
		<strong class="stat-value">{formatNumber(votes_up)}</strong>
	</div>

	<div class="stat-content">
		<span class="stat-label">Downvotes</span>
		<strong class="stat-value">{formatNumber(votes_down)}</strong>
	</div>
</div>

<style>
	.voting-card {
		display: flex;
		flex-wrap: wrap;
		gap: 1rem;
		padding: 1rem;
		background: rgba(255, 255, 255, 0.08);
		border: 1px solid rgba(255, 255, 255, 0.12);
		border-radius: 0.85rem;
		backdrop-filter: blur(10px);
		justify-content: space-evenly;
	}

	.stat-content {
		display: flex;
		flex-direction: column;
		gap: 0.2rem;
		justify-content: center;
		align-items: center;
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
</style>
