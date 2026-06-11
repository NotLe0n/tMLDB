<script lang="ts">
	import { formatDate, formatDuration, formatNumber, type ModData } from "$lib";

	const { mod }: { mod: ModData } = $props()
</script>

<div class="comparsion panel">
	<span>Display Name</span>
	<span>{mod.display_name}</span>
	<span>Internal Name</span>
	<span>{mod.internal_name}</span>
	<span>Downloads</span>
	<span>{formatNumber(mod.downloads_total)}</span>
	<span>Favorites</span>
	<span>{formatNumber(mod.favorited)}</span>
	<span>Playtime</span>
	<span>{formatDuration(Number.parseInt(mod.playtime))}</span>
	<span>Views</span>
	<span>{formatNumber(mod.views)}</span>
	<span>Comments</span>
	<span>{formatNumber(mod.num_comments)}</span>
	<span>Tags</span>
	<span>{mod.tags?.map(x => x.display_name).join(', ')}</span>
	<span>Time created</span>
	<span>{formatDate(mod.time_created)}</span>
	<span>Last update</span>
	<span>{formatDate(mod.time_updated)}</span>
	<span>Steam Rating</span>
	<span>{((mod.vote_data?.score || 0) * 100).toFixed(4)}%</span>
	<span>Upvotes</span>
	<span>{formatNumber(mod.vote_data?.votes_up || 0)}</span>
	<span>Downvotes</span>
	<span>{formatNumber(mod.vote_data?.votes_down || 0)}</span>
</div>

<style>
	.comparsion {
		display: grid;
		grid-template-columns: 1fr 3fr;
	}

	.comparsion > span {
		padding: 0.25rem 0;

		&:nth-child(2n+1) {
			font-weight: bold;
		}
	}
</style>
