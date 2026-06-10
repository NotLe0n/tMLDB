<script lang="ts">
	import { formatNumber, formatNumberShort } from "$lib";

	let { votes_up, votes_down, score } = $props();

	const percentage = $derived((score * 100).toFixed(2));
</script>

<div class="voting-card panel">
	<div class="stat-content">
		<span class="stat-label">
			Steam Rating
			<span class="info-icon" title="Steam's rating may differ from the upvote/downvote ratio">?</span>
		</span>
		<strong class="stat-value">{percentage}%</strong>
	</div>

	<div class="stat-content" title="{formatNumber(votes_up)} Upvotes">
		<span class="stat-label">Upvotes</span>
		<strong class="stat-value">{formatNumberShort(votes_up)}</strong>
	</div>

	<div class="stat-content" title="{formatNumber(votes_down)} Downvotes">
		<span class="stat-label">Downvotes</span>
		<strong class="stat-value">{formatNumberShort(votes_down)}</strong>
	</div>
</div>

<style>
	.voting-card {
		display: flex;
		flex-wrap: wrap;
		gap: 1rem;
		padding: 1rem;
		border-radius: 0.75rem;
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

	.info-icon {
		border: 1px solid white;
		border-radius: 50%;
		margin-left: 5px;
		padding-left: 5px;
		padding-right: 4px;
		text-align: center;
	}
</style>
