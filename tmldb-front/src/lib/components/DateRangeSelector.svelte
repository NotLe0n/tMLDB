<script lang="ts">
	import { DATE_RANGES, type DateRange } from "$lib";

	let dateSelectorElm: HTMLDivElement;

	type DateRangeSelectorProps = { dates: string[], beginDate: string, endDate: string }
	let { dates, beginDate = $bindable(), endDate = $bindable() }: DateRangeSelectorProps = $props();
	
	if (beginDate === "") {
		// svelte-ignore state_referenced_locally
		beginDate = dates[0];
	}
	if (endDate === "") {
		// svelte-ignore state_referenced_locally
		endDate = dates[dates.length - 1];
	}

	let beginIndex = $derived(dates.indexOf(beginDate))
	let endIndex = $derived(dates.indexOf(endDate))

	const clamp = (val: number, min: number, max: number) => Math.min(Math.max(val, min), max)

	let dragging = $state(0); // 0: none, 1: begin, 2: end
	const MIN_DIFF = 4;

	function clientXToIndex(clientX: number): number {
		const rect = dateSelectorElm.getBoundingClientRect();
		const percent = clamp((clientX - rect.x) / rect.width, 0, 1);
		return Math.round((dates.length-1) * percent);
	}

	function moveHandle(clientX: number) {
		if (dragging === 0) return;
		const index = clientXToIndex(clientX);
		if (dragging === 1) {
			beginDate = dates[clamp(index, 0, endIndex - MIN_DIFF)];
		} else {
			endDate = dates[clamp(index, beginIndex + MIN_DIFF, dates.length-1)];
		}
	}

	function onKeyDown(handle: 1 | 2, event: KeyboardEvent) {
		if (event.key !== 'ArrowLeft' && event.key !== 'ArrowRight') return;
		event.preventDefault();
		const delta = event.key === 'ArrowLeft' ? -1 : 1;
		if (handle === 1) {
			beginDate = dates[clamp(beginIndex + delta, 0, endIndex - MIN_DIFF)];
		} else {
			endDate = dates[clamp(beginIndex - delta, beginIndex + MIN_DIFF, dates.length-1)];
		}
	}

	function selectPreset(ev: Event) {
		const range = (ev.target as HTMLSelectElement).value as DateRange
		let selectedDate = new Date()
		selectedDate.setDate(selectedDate.getDate() - 1)

		switch (range) {
			case "Full":
				selectedDate = new Date(dates[0])
				break;
			case "Year":
				selectedDate.setUTCFullYear(selectedDate.getUTCFullYear() - 1)
				break;
			case "4 Months":
				selectedDate.setMonth(selectedDate.getMonth() - 4)
				break;
			case "Month":
				selectedDate.setMonth(selectedDate.getMonth() - 1)
				break;
			case "2 Weeks":
				selectedDate.setDate(selectedDate.getDate() - 14)
				break;
			case "Week":
				selectedDate.setDate(selectedDate.getDate() - 7)
				break;
		}

		beginDate = dates.find(x => new Date(x) >= selectedDate) || dates[0]
		endDate = dates[dates.length - 1]

	}

	let startPct = $derived((beginIndex / dates.length) * 100);
	let endPct = $derived((endIndex / (dates.length-1)) * 100);
</script>

<svelte:window
	onmousemove={(e) => moveHandle(e.clientX)}
	onmouseup={() => (dragging = 0)}
	ontouchmove={(e) => moveHandle(e.touches[0].clientX)}
	ontouchend={() => (dragging = 0)}
/>

<div class="date-range-container">
	<select class="date-range-selector" onchange={selectPreset}>
		{#each DATE_RANGES as range}
			<option value={range}>{range}</option>
		{/each}
	</select>

	<div bind:this={dateSelectorElm} class="date-range-slider" class:dragging={dragging !== 0}>
		<div class="track">
			<div 
				class="track-fill" 
				style="left: {startPct}%; width: {endPct - startPct}%">
			</div>
		</div>

		<button
			class="handle handle-begin"
			style="left: {startPct}%; anchor-name: --handle-begin"
			onmousedown={() => { dragging = 1; } }
			ontouchstart={() => { dragging = 1; }}
			onkeydown={(e) => onKeyDown(1, e)}
			role="slider"
			aria-label="Start date"
			aria-valuenow={beginIndex}
			aria-valuemin={0}
			aria-valuemax={endIndex - MIN_DIFF}
			tabindex={0}
		></button>

		<button
			class="handle handle-end"
			style="left: {endPct}%; anchor-name: --handle-end"
			onmousedown={() => { dragging = 2; }}
			ontouchstart={() => { dragging = 2; }}
			onkeydown={(e) => onKeyDown(2, e)}
			role="slider"
			aria-label="End date"
			aria-valuenow={endIndex}
			aria-valuemin={beginIndex + MIN_DIFF}
			aria-valuemax={dates.length-1}
			tabindex={0}
		></button>

		<span class="handle-label label-begin">{beginDate}</span>
		<span class="handle-label label-end">{endDate}</span>
	</div>
</div>

<style>
	.date-range-container {
		display: flex;
		align-items: center;
		gap: 1rem;
	}

	.date-range-selector {
		height: 1.5rem;
		background-color: var(--tertiary-bg);
		border: 2px solid var(--primary);
		border-radius: 0.5rem;
		cursor: pointer;
		transition: background-color  0.3s;

		&:hover {
			background-color: var(--tertiary-hov);
		}

		& > option {
			background-color: var(--tertiary);
		}
	}

	.date-range-slider {
		position: relative;
		width: 100%;
		height: 2.5rem;
		display: flex;
		align-items: center;

		&.dragging {
			cursor: ew-resize;
		}
	}

	.track {
		position: absolute;
		width: 100%;
		height: 1rem;
		background-color: var(--tertiary-bg);
		border: 1px solid color-mix(in srgb, var(--primary) 40%, transparent);
		border-radius: 0.25rem;
	}

	.track-fill {
		position: absolute;
		top: 0;
		height: 100%;
		background-color: var(--primary);
		opacity: 0.5;
	}

	.handle {
		position: absolute;
		transform: translateX(-50%);
		width: 14px;
		height: 1.5rem;
		background-color: var(--tertiary);
		border: 2px solid var(--primary);
		border-radius: 0.25rem;
		cursor: ew-resize;
		padding: 0;
		display: grid;
		place-items: center;
		transition: background-color 0.1s, border-color 0.1s;

		/* grip lines */
		&::before {
			content: '';
			display: block;
			width: 2px;
			height: 10px;
			border-left: 1px solid white;
			border-right: 1px solid white;
			opacity: 0.5;
		}
	}

	.handle-label {
		position: absolute;
		position-area: bottom;
		top: 1.25rem;
		background-color: var(--tertiary-bg);
		border: 1px solid color-mix(in srgb, var(--primary) 50%, transparent);
		border-radius: 4px;
		padding: 2px 6px;
		font-size: 0.7rem;
	}

	.label-begin { position-anchor: --handle-begin; }
	.label-end   { position-anchor: --handle-end; }
</style>
