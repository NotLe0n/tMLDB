<script lang="ts">
    import { formatDuration, formatNumber, type ModHistoryData } from "$lib";

	const { data, beginDate, endDate }: { data: ModHistoryData, beginDate: string, endDate: string } = $props();

	const startIndex = $derived(data.dates.indexOf(beginDate))
	const endIndex = $derived(data.dates.indexOf(endDate))

	const dl = $derived(data.downloads.slice(startIndex, endIndex))
	const vs = $derived(data.views.slice(startIndex, endIndex))
	const fv = $derived(data.favorited.slice(startIndex, endIndex))
	const pt = $derived(data.playtime.slice(startIndex, endIndex))
	const vu = $derived(data.votes_up.slice(startIndex, endIndex))
	const vd = $derived(data.votes_down.slice(startIndex, endIndex))

	function formatNumberWithSign(value: number) {
		return new Intl.NumberFormat('en-US', { notation: 'standard', 'signDisplay': "exceptZero" }).format(value)
	}
</script>
<table>
	<thead>
		<tr>
			<td>Date</td>
			<td>Downloads</td>
			<td>Views</td>
			<td>Favorites</td>
			<td>Playtime</td>
			<td>Upvotes</td>
			<td>Downvotes</td>
		</tr>
	</thead>
	<tbody>
		{#each data.dates.slice(startIndex, endIndex) as date, i}
			<tr>
				<td>{date}</td>
				<td class={{neg: dl[i] < 0, pos: dl[i] > 0}}>
					{formatNumber(dl[i])}
				</td>
				<td class={{neg: vs[i] < 0, pos: vs[i] > 0}}>
					{formatNumberWithSign(vs[i])}
				</td>
				<td class={{neg: fv[i] < 0, pos: fv[i] > 0}}>
					{formatNumberWithSign(fv[i])}
				</td>
				<td class={{neg: pt[i] < 0, pos: pt[i] > 0}}>
					+{formatDuration(pt[i])}
				</td>
				<td class={{neg: vu[i] < 0, pos: vu[i] > 0}}>
					{formatNumberWithSign(vu[i])}
				</td>
				<!-- reversed on purpose -->
				<td class={{neg: vd[i] > 0, pos: vd[i] < 0}}>
					{formatNumberWithSign(vd[i])}
				</td>
			</tr>
		{/each}
	</tbody>
</table>

<style>
	table {
		width: 100%;
		min-width: 40rem;
		text-align: center;
		margin-top: 1rem;
	}

	thead {
		background-color: var(--panel-col3);
		font-weight: bold;

		& > tr > td {
			padding: 0.5rem 0;
		}
	}

	tbody > tr {
		background-color: var(--panel-col);
		border-top: 1px solid var(--panel-col3);

		& > td {
			padding: 0.5rem 0;

			&.pos {
				color: oklch(from var(--upvote-color) calc(l + 0.3) c h);
			}
			&.neg {
				color: var(--downvote-color);
			}
		}
	}
</style>
