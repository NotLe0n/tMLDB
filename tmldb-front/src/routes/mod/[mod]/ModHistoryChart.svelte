<script lang="ts">
	import { S_TO_HRS, S_TO_MIN, msToDuration, type ModHistoryData, type DateRange, S_TO_MTH } from '$lib';
	import Chart from 'chart.js/auto';
	import type { Chart as ChartType } from 'chart.js/auto';
	import annotationPlugin, { type AnnotationOptions } from 'chartjs-plugin-annotation';

	Chart.register(annotationPlugin);

	let { data, beginDate, endDate }: { data: ModHistoryData, beginDate: string, endDate: string } = $props();
	let chartInstance: ChartType | null = $state(null);

	let startIndex = $derived(data.dates.indexOf(beginDate))
	let endIndex = $derived(data.dates.indexOf(endDate))

	function convertPlaytime(playtime: number[]): [number[], string] {
		const min = Math.min(...playtime);
		const minDur = msToDuration(min);

		if (minDur.months > 0) return [playtime.map(x => x / S_TO_MTH), "months"]
		if (minDur.hours > 0) return [playtime.map(x => x / S_TO_HRS), "hours"]
		if (minDur.minutes > 0) return [playtime.map(x => x / S_TO_MIN), "minutes"]

		return [playtime, "seconds"]
	}
	const convertedPlaytime = $derived(convertPlaytime(data.playtime))

	const palette = {
		subscriptions: 'rgb(255, 235, 59)',
		views: 'rgb(59, 130, 246)',
		favorited: 'rgb(251, 146, 60)',
		playtime: 'rgb(168, 85, 247)',
		votesUp: 'rgb(34, 197, 94)',
		votesDown: 'rgb(239, 68, 68)'
	};

	function getUpdateAnnotation(time: number, version: string): AnnotationOptions {
		const date = new Date(time * 1000).toISOString().split('T')[0];
		return {
			type: 'line',
			xMin: date,
			xMax: date,
			borderColor: 'rgb(75, 192, 192)',
			borderWidth: 1,
			label: {
				display: true,
				content: `Update ${version ? "on " + version : ""}`,
				position: 'end',
				rotation: 90,
				backgroundColor: 'transparent',
				xAdjust: 10,
			}
		}
	}

	function convertDate(time: string | number): string{
		return new Date(time).toLocaleDateString('en-US', {
			month: 'short',
			day: '2-digit'
		})
	}

	function getDatasets() {
		return [
			{
				label: 'Subscriptions',
				data: data.subscriptions.slice(startIndex, endIndex),
				borderColor: palette.subscriptions,
				backgroundColor: palette.subscriptions
			},
			{
				label: 'Views',
				data: data.views.slice(startIndex, endIndex),
				borderColor: palette.views,
				backgroundColor: palette.views
			},
			{
				label: 'Favorites',
				data: data.favorited.slice(startIndex, endIndex),
				borderColor: palette.favorited,
				backgroundColor: palette.favorited
			},
			{
				label: 'Playtime',
				data: convertedPlaytime[0].slice(startIndex, endIndex),
				borderColor: palette.playtime,
				backgroundColor: palette.playtime,
			},
			{
				label: 'Upvotes',
				data: data.votes_up.slice(startIndex, endIndex),
				borderColor: palette.votesUp,
				backgroundColor: palette.votesUp,
				yAxisID: 'y1',
				hidden: true
			},
			{
				label: 'Downvotes',
				data: data.votes_down.slice(startIndex, endIndex),
				borderColor: palette.votesDown,
				backgroundColor: palette.votesDown,
				yAxisID: 'y1',
				hidden: true
			}
		]
	}

	$effect(() => {
		if (!chartInstance) return;

		chartInstance.data.labels = data.dates.slice(startIndex, endIndex);
		chartInstance.data.datasets[0].data = data.subscriptions.slice(startIndex, endIndex);
		chartInstance.data.datasets[1].data = data.views.slice(startIndex, endIndex);
		chartInstance.data.datasets[2].data = data.favorited.slice(startIndex, endIndex);
		chartInstance.data.datasets[3].data = convertedPlaytime[0].slice(startIndex, endIndex);
		chartInstance.data.datasets[4].data = data.votes_up.slice(startIndex, endIndex);
		chartInstance.data.datasets[5].data = data.votes_down.slice(startIndex, endIndex);

		(chartInstance.options.plugins as any).annotation.annotations = data.time_updated
			.filter(t => t * 1000 >= new Date(data.dates[startIndex]).getTime() && (endIndex === undefined || t*1000 < new Date(data.dates[endIndex]).getTime()))
			.map((v, i) => getUpdateAnnotation(v, data.version[i]));

		chartInstance.update('none');
	});

	function chart(node: HTMLCanvasElement) {
		chartInstance = new Chart(node, {
			type: 'line',
			data: {
				labels: data.dates.slice(startIndex, endIndex),
				datasets: getDatasets()
			},
			options: {
				responsive: true,
				maintainAspectRatio: false,
				interaction: {
					intersect: false,
					mode: 'index'
				},
				datasets: {
					line: {
						borderWidth: 2,
						pointStyle: 'circle',
						pointRadius: 1,
						tension: 0.2
					}
				},
				plugins: {
					legend: {
						labels: {
							color: 'rgba(255, 255, 255, 0.8)'
						}
					},
					tooltip: {
						callbacks: {
							label: function(context) {
								const label = context.dataset.label || '';
								const value = context.parsed.y;

								// Format playtime as human-readable duration
								if (context.datasetIndex === 3) { // playtime is at index 3
									return `${label}: ${value?.toFixed(2)} ${convertedPlaytime[1]}`;
								}

								return `${label}: ${value?.toLocaleString()}`;
							}
						}
					}
				},
				scales: {
					x: {
						ticks: {
							color: 'rgba(255, 255, 255, 0.6)',
							callback: function (value) {
								const label = this.getLabelForValue(value as number);
								if (!label) return '';
								return convertDate(label);
							}
						},
						grid: {
							color: 'rgba(255, 255, 255, 0.06)'
						}
					},
					y: {
						type: 'linear',
						display: true,
						position: 'left',
						title: {
							display: true,
							text: 'Subscriptions / Views / Favorites / Playtime',
							color: 'rgba(255, 255, 255, 0.7)'
						},
						ticks: {
							color: 'rgba(255, 255, 255, 0.6)'
						},
						grid: {
							color: 'rgba(255, 255, 255, 0.06)'
						}
					},
					y1: {
						type: 'linear',
						display: true,
						position: 'right',
						title: {
							display: true,
							text: 'Votes',
							color: 'rgba(255, 255, 255, 0.7)'
						},
						ticks: {
							color: 'rgba(255, 255, 255, 0.6)'
						},
						grid: {
							display: false
						}
					}
				}
			}
		});

		return {
			destroy() {
				chartInstance?.destroy();
			}
		};
	}
</script>

<canvas class="chart" use:chart></canvas>

<style>
	.chart {
		width: 100%;
		height: 520px;
	}
</style>
