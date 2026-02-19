<script lang="ts">
	import LoadingSpinner from "$lib/components/LoadingSpinner.svelte";

	interface OnDetectCallback { (): Promise<void> }
	type ScrollDetectorProps = { loading: boolean, onDetect: OnDetectCallback }
	
	let { loading = $bindable(), onDetect } : ScrollDetectorProps = $props()

	let scrollDetector: Element;
	$effect(() => infiniteScroll(scrollDetector))

	const infiniteScroll = (element: Element) => {
		if (element) {
			const observer = new IntersectionObserver(async (entries) => {
			const first = entries[0];
				if (first.isIntersecting && !loading) {
					loading = true;
					await onDetect();
					loading = false;
				}
			});

			observer.observe(element);
		}
	};
</script>

<div id="scroll-detector" bind:this={scrollDetector}>
	{#if loading}
		<LoadingSpinner size=24 />
	{/if}
</div>

<style>
	#scroll-detector {
		width: 100%;
		margin: 1rem 0;
	}
</style>