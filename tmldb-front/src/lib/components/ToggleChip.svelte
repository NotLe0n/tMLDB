<script lang="ts">
	type ToggleChipProps = { value: boolean,label1: string, label2: string }
	let { value = $bindable(), label1, label2 }: ToggleChipProps = $props() 
</script>

<div class="toggle-chip" class:pos2={!value}>
	<button onclick={() => value = true} name="chip-pos1">{label1}</button>
	<button onclick={() => value = false} name="chip-pos2">{label2}</button>
</div>

<style>
	.toggle-chip {
		position: relative;
		height: 1.75rem;
		width: 8rem;
		
		display: flex;
		justify-content: space-between;
		border-radius: 1rem;
		border: 1px solid var(--green3);
		background-color: var(--green3-bg);
		transition: background-color 0.3s;

		& > button {
			appearance: none;
			background-color: transparent;
			color: var(--highlight);
			mix-blend-mode: difference;
			border: none;
			padding: 0 1rem;
			cursor: pointer;
		}

		&:hover {
			background-color: var(--green3-hov);
		}

		&::before {
			content: '';
			position: absolute;
			left: 0;
			width: 50%;
			height: 100%;
			background-color: var(--highlight);
			transition: left 0.2s ease-in-out;
			border-radius: 1rem;
		}

		&.pos2::before {
			left: 50%;
		}
	}
</style>