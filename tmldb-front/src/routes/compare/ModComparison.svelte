<script lang="ts">
	import { formatDate, formatDuration, formatNumber, type ModData } from "$lib";

	const { mod1, mod2 }: { mod1: ModData | null, mod2: ModData | null } = $props()

	type DiffResult = { text: string; cls: 'pos' | 'neg' | '' } | null;

	function numDiff(a: number | undefined, b: number | undefined): DiffResult {
		if (a == null || b == null) return null;
		if (a === 0 || b === 0) return { text: 'N/A', cls: '' };

		return { text: `${formatNumber(b - a)}`, cls: (b - a) > 0 ? 'pos' : 'neg' };
	}

	function percDiff(a: number | undefined, b: number | undefined): DiffResult {
		if (a == null || b == null) return null;
		if (a === 0 || b === 0) return { text: 'N/A', cls: '' };

		const pct = ((b - a) / Math.abs(a)) * 100;
		if (Math.abs(pct) < 0.05) return { text: '≈0%', cls: '' };
		return { text: `${pct.toFixed(1)}%`, cls: pct > 0 ? 'pos' : 'neg' };
	}

	function ratingDiff(a: number | undefined, b: number | undefined): DiffResult {
		if (a == null || b == null) return null;

		const diff = (b - a) * 100;
		if (Math.abs(diff) < 0.005) return { text: '≈0', cls: '' };
		return { text: `${diff.toFixed(2)}pp`, cls: diff > 0 ? 'pos' : 'neg' };
	}

	function v(mod: ModData | null, val: string | number | undefined | null, fallback = ''): string {
		if (mod === null) return '';
		return val != null ? String(val) : fallback;
	}
</script>

{#snippet diff(d: DiffResult)}
	{#if d}
		<span class="diff {d.cls}" title="relative to {mod1?.display_name ?? 'Mod 1'}">{d.text}</span>
	{:else}
		<span></span>
	{/if}
{/snippet}

<div class="comparison panel">
	<div>
		<div class="mod1-cell">
			<span class="label"></span>
			<span class="col-header">{mod1?.display_name ?? 'Select a mod to compare to'}</span>
		</div>
		<span class="diff-header">Δ</span>
		<span class="col-header">{mod2?.display_name ?? 'Select a mod to compare to'}</span>

		<div class="mod1-cell">
			<span class="label">Internal Name</span>
			<span>{v(mod1, mod1?.internal_name)}</span>
		</div>
		<span class="no-diff">{v(mod2, mod2?.internal_name)}</span>

		<div class="mod1-cell">
			<span class="label">Subscriptions</span>
			<span>{mod1 ? formatNumber(mod1.subscriptions_total) : ''}</span>
		</div>
		{@render diff(percDiff(mod1?.subscriptions_total, mod2?.subscriptions_total))}
		<span>{mod2 ? formatNumber(mod2.subscriptions_total) : ''}</span>

		<div class="mod1-cell">
			<span class="label">Favorites</span>
			<span>{mod1 ? formatNumber(mod1.favorited) : ''}</span>
		</div>
		{@render diff(percDiff(mod1?.favorited, mod2?.favorited))}
		<span>{mod2 ? formatNumber(mod2.favorited) : ''}</span>

		<div class="mod1-cell">
			<span class="label">Views</span>
			<span>{mod1 ? formatNumber(mod1.views) : ''}</span>
		</div>
		{@render diff(percDiff(mod1?.views, mod2?.views))}
		<span>{mod2 ? formatNumber(mod2.views) : ''}</span>

		<div class="mod1-cell">
			<span class="label">Playtime</span>
			<span>{mod1 ? formatDuration(Number.parseInt(mod1.playtime)) : ''}</span>
		</div>
		{@render diff(percDiff(mod1 ? Number.parseInt(mod1.playtime) : undefined, mod2 ? Number.parseInt(mod2.playtime) : undefined))}
		<span>{mod2 ? formatDuration(Number.parseInt(mod2.playtime)) : ''}</span>

		<div class="mod1-cell">
			<span class="label">Comments</span>
			<span>{mod1 ? formatNumber(mod1.num_comments) : ''}</span>
		</div>
		{@render diff(numDiff(mod1?.num_comments, mod2?.num_comments))}
		<span>{mod2 ? formatNumber(mod2.num_comments) : ''}</span>

		<div class="mod1-cell">
			<span class="label">Tags</span>
			<span class="tags">{mod1?.tags?.map(x => x.display_name).toSorted().join(', ') ?? ''}</span>
		</div>
		<span class="tags no-diff">{mod2?.tags?.map(x => x.display_name).toSorted().join(', ') ?? ''}</span>

		<div class="mod1-cell">
			<span class="label">Steam Rating</span>
			<span>{mod1 ? ((mod1.vote_data?.score ?? 0) * 100).toFixed(4) + '%' : ''}</span>
		</div>
		{@render diff(ratingDiff(mod1?.vote_data?.score, mod2?.vote_data?.score))}
		<span>{mod2 ? ((mod2.vote_data?.score ?? 0) * 100).toFixed(4) + '%' : ''}</span>

		<div class="mod1-cell">
			<span class="label">Upvotes</span>
			<span>{mod1 ? formatNumber(mod1.vote_data?.votes_up ?? 0) : ''}</span>
		</div>
		{@render diff(numDiff(mod1?.vote_data?.votes_up, mod2?.vote_data?.votes_up))}
		<span>{mod2 ? formatNumber(mod2.vote_data?.votes_up ?? 0) : ''}</span>

		<div class="mod1-cell">
			<span class="label">Downvotes</span>
			<span>{mod1 ? formatNumber(mod1.vote_data?.votes_down ?? 0) : ''}</span>
		</div>
		{@render diff(numDiff(mod1?.vote_data?.votes_down, mod2?.vote_data?.votes_down))}
		<span>{mod2 ? formatNumber(mod2.vote_data?.votes_down ?? 0) : ''}</span>

		<div class="mod1-cell">
			<span class="label">Created</span>
			<span>{mod1 ? formatDate(mod1.time_created) : ''}</span>
		</div>
		<span class="no-diff">{mod2 ? formatDate(mod2.time_created) : ''}</span>

		<div class="mod1-cell">
			<span class="label">Last Update</span>
			<span>{mod1 ? formatDate(mod1.time_updated) : ''}</span>
		</div>
		<span class="no-diff">{mod2 ? formatDate(mod2.time_updated) : ''}</span>
	</div>
</div>

<style>
	.comparison {
		overflow: visible;
	}

	.comparison > div {
		display: grid;
		grid-template-columns: 1fr auto 1fr;
		column-gap: clamp(0.1rem, 3vw, 3rem);
		padding: 0.75rem 1rem;
		overflow-y: auto;
		max-height: calc(100vh - var(--nav-height) - var(--footer-height) - 3rem - 8rem);
	}

	.comparison > div > :is(span:not(.diff), div) {
		padding: 0.3rem 0.25rem;
		&:not(:last-of-type) {
			border-bottom: 1px solid color-mix(in srgb, currentColor 8%, transparent);
		}
	}

	.mod1-cell {
		display: flex;
		align-items: flex-start;
		gap: 0.75rem;
	}

	.no-diff {
		grid-column: 3;
	}

	.label {
		font-weight: bold;
		font-size: 0.85em;
		opacity: 0.6;
		width: 7rem;
		flex-shrink: 0;
		padding: 0;
	}

	.col-header {
		font-weight: bold;
		font-size: 0.95em;
	}

	.tags {
		white-space: normal;
		line-height: 1.5;
	}

	.diff-header {
		font-size: 0.7em;
		opacity: 0.4;
		text-align: center;
		align-self: center;
	}

	.diff {
		font-size: 0.85em;
		font-weight: 600;
		padding: 0.15rem 0.45rem;
		border-radius: 0.9rem;
		align-self: center;
		text-align: center;
	}

	.diff.pos {
		&::before {
			content: '+';
		}
		color: #4ade80;
		background: color-mix(in srgb, #4ade80 12%, transparent);
	}

	.diff.neg {
		color: #f87171;
		background: color-mix(in srgb, #f87171 12%, transparent);
	}
</style>
