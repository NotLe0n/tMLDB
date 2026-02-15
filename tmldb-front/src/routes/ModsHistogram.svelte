<script lang="ts">
	import Chart from "chart.js/auto"
	import type { Chart as ChartType } from "chart.js/auto"

	type LineChartData = {
		labels: any[],
		data: any[]
	}

	let { title, data }: {title:string, data: LineChartData} = $props();
	let chartInstance: ChartType | null = $state(null);

	function chart(node: any) {
		chartInstance = new Chart(
			node, {
				type: 'line',
				options: {
					interaction: {
						intersect: false
					},
					responsive: true,
					maintainAspectRatio: false,
					datasets: {
						line: {
							borderWidth: 1,
							pointStyle: false,
							tension: .2
						}
					},
					scales: {
						x: {
							ticks: {
								callback: function(value) {
									const label = this.getLabelForValue(value as number);
									if (!label) return '';
									return new Date(label).toLocaleDateString('en-US', {
										year: 'numeric',
										month: 'short'
									});
								}
							}
						}
					}
				},
				data: {
					labels: data.labels,
					datasets: [
						{
							label: title,
							data: data.data
						}
					]
				}
			},
		);

		return {
			destroy() {
				chartInstance?.destroy();
			}
		};
	};

	$effect(() => {
		if (chartInstance && data) {
			chartInstance.data.labels = data.labels;
			chartInstance.data.datasets[0].data = data.data;
			chartInstance.update();
		}
	});
</script>

<canvas class="chart" use:chart></canvas>
