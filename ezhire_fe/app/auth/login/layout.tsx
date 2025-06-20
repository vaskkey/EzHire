import type { Metadata } from "next";
import { PropsWithChildren } from "react";

export const metadata: Metadata = {
	title: "EzHire - Kampanie",
	description: "Zarządzaj swoimi kampaniami",
};

export default function RootLayout({ children }: PropsWithChildren) {
	return <main className="bg-base-200 min-h-screen lg:p-6 p-2">{children}</main>;
}
