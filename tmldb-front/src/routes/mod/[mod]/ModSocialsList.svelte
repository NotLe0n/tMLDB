<script lang="ts">
	import { mdiCube, mdiFacebook, mdiReddit, mdiSteam, mdiTwitter, mdiYoutube } from '@mdi/js';
	import Icon from '$lib/components/Icon.svelte';

	type ModSocials = {
		youtube?: string | null,
		twitter?: string | null,
		reddit?: string | null,
		facebook?: string | null,
		sketchfab?: string | null
	}

	const { socials, mod_id }: { socials: ModSocials | null, mod_id: number } = $props()
</script>

<div id="socials-list">
	<a href={`https://steamcommunity.com/sharedfiles/filedetails/?id=${mod_id}`} title="Mod Steam page">
		<Icon path={mdiSteam} />
	</a>

	{#if socials}
		{#if socials.youtube}
			<a href={socials.youtube} title="Mod YouTube page"><Icon path={mdiYoutube} /></a>
		{/if}
		{#if socials.twitter}
			<a href={socials.twitter} title="Mod Twitter page"><Icon path={mdiTwitter} /></a>
		{/if}
		{#if socials.facebook}
			<a href={socials.facebook} title="Mod Facebook page"><Icon path={mdiFacebook} /></a>
		{/if}
		{#if socials.reddit}
			<a href={socials.reddit} title="Mod Reddit page"><Icon path={mdiReddit} /></a>
		{/if}
		{#if socials.sketchfab}
			<a href={socials.sketchfab} title="Mod Sketchfab page"><Icon path={mdiCube} /></a>
		{/if}
	{/if}
</div>

<style>
	#socials-list {
		display: flex;
		gap: 1rem;
	}
</style>