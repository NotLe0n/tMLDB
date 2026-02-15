declare module '@jamescoyle/svelte-icon' {
	import { SvelteComponentTyped } from "svelte";

	declare module SvgIcon {
		export interface SvgIconProps {
			/**
			 * Required. An SVG path to render as an icon
			 */
			path: string;
			/**
			 * This sets the size and viewbox to match the recommended size for the icon pack specified.
			 */
			type?: "mdi" | "simple-icons" | "default";
			/**
			 * The width and height of the SVG element
			 */
			size?: string | number;
			/**
			 * The `viewBox` of the SVG element
			 */
			viewbox?: string;
			/**
			 * One of "none", "horizontal", "vertical", or "both". Flips the icon in the specified direction(s).
			 */
			flip?: "none" | "horizontal" | "vertical" | "both";
			/**
			 * Rotates the icon by the specified value. Can be any valid [CSS angle](https://developer.mozilla.org/en-US/docs/Web/CSS/angle) value.
			 */
			rotate?: string | number;
		}
	}

	type Props = SvgIcon.SvgIconProps & { [attribute: string]: any };

	declare class SvgIcon extends SvelteComponentTyped<Props> {}
	export = SvgIcon;
}
