<script lang="ts">
	import { MS_TO_HRS, MS_TO_MIN, MS_TO_SEC, msToDuration, type ModHistoryData } from '$lib';
	import Chart from 'chart.js/auto';
	import type { Chart as ChartType } from 'chart.js/auto';
	import annotationPlugin, { type AnnotationOptions } from 'chartjs-plugin-annotation';

	Chart.register(annotationPlugin);

	let { data }: { data: ModHistoryData } = $props();
	let chartInstance: ChartType | null = $state(null);
	const startRange = $derived(new Date(data.dates[0]))

	function convertPlaytime(playtime: number[]): [number[], string] {
		const min = Math.min(...playtime);
		const minDur = msToDuration(min);
		
		if (minDur.hours > 0) return [playtime.map(x => x / MS_TO_HRS), "hours"]
		if (minDur.minutes > 0) return [playtime.map(x => x / MS_TO_MIN), "minutes"]

		return [playtime.map(x => x / MS_TO_SEC), "seconds"]
	}
	const convertedPlaytime = $derived(convertPlaytime(data.playtime))

	const palette = {
		downloads: 'rgb(132, 204, 22)',
		views: 'rgb(59, 130, 246)',
		favorited: 'rgb(251, 146, 60)',
		playtime: 'rgb(168, 85, 247)',
		votesUp: 'rgb(34, 197, 94)',
		votesDown: 'rgb(239, 68, 68)'
	};
	
	function getUpdateAnnotation(time: number): AnnotationOptions {
		const date = new Date(time * 1000).toISOString().split('T')[0];
		return {
			type: 'line',
			xMin: date,
			xMax: date,
			borderColor: 'rgb(75, 192, 192)',
			borderWidth: 1,
			label: {
				display: true,
				content: 'Update',
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

	function chart(node: HTMLCanvasElement) {
		chartInstance = new Chart(node, {
			type: 'line',
			data: {
				labels: data.dates,
				datasets: [
					{
						label: 'Downloads',
						data: data.downloads,
						borderColor: palette.downloads,
						backgroundColor: palette.downloads
					},
					{
						label: 'Views',
						data: data.views,
						borderColor: palette.views,
						backgroundColor: palette.views
					},
					{
						label: 'Favorites',
						data: data.favorited,
						borderColor: palette.favorited,
						backgroundColor: palette.favorited
					},
					{
						label: 'Playtime',
						data: convertedPlaytime[0],
						borderColor: palette.playtime,
						backgroundColor: palette.playtime,
					},
					{
						label: 'Upvotes',
						data: data.votes_up,
						borderColor: palette.votesUp,
						backgroundColor: palette.votesUp,
						yAxisID: 'y1',
						hidden: true
					},
					{
						label: 'Downvotes',
						data: data.votes_down,
						borderColor: palette.votesDown,
						backgroundColor: palette.votesDown,
						yAxisID: 'y1',
						hidden: true
					}
				]
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
					},
					annotation: {
						annotations: data.time_updated
							.filter(t => t*1000 >= startRange.getTime())
							.map(getUpdateAnnotation)
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
							text: 'Downloads / Views / Favorites / Playtime',
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
