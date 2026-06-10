<script lang="ts">
	type ToggleChipProps = { value: boolean,label1: string, label2: string }
	let { value = $bindable(), label1, label2 }: ToggleChipProps = $props()
</script>

<div class="toggle-chip panel" >
	<div class:pos2={!value} class="indicator panel-medium"></div>
	<button onclick={() => value = true} name="chip-pos1" class:selected={value}>{label1}</button>
	<button onclick={() => value = false} name="chip-pos2" class:selected={!value}>{label2}</button>
</div>

<style>
	.toggle-chip {
		position: relative;
		height: 1.75rem;
		width: 8rem;

		display: flex;
		justify-content: space-between;
		transition: background-color 0.3s;

		&:not(:has(.selected:hover)):hover {
			background-color: var(--panel-col2);
		}
	}

	.toggle-chip > button {
		appearance: none;
		background-color: transparent;
		border: none;
		padding: 0 1rem;
		transition: 0.5s color ease;

		&::after {
			display: none;
		}

		&.selected {
			color: var(--yellow)
		}
	}



	.indicator {
		position: absolute;
		left: -2px;
		top: -2px;
		width: 50%;
		height: 100%;
		background-color: var(--panel-col3);
		transition: left 0.2s ease-in-out;
	}

	.pos2 {
		left: 50%;
	}
</style>
