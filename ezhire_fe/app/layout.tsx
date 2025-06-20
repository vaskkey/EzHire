import type { Metadata } from "next";
import { Geist, Geist_Mono } from "next/font/google";
import "./globals.css";
import { PropsWithChildren } from "react";

const geistSans = Geist({
	variable: "--font-geist-sans",
	subsets: ["latin"],
});

const geistMono = Geist_Mono({
	variable: "--font-geist-mono",
	subsets: ["latin"],
});

export const metadata: Metadata = {
	title: "EzHire",
	description: "Jedno miejsce na zarządzanie twoimi rekrutacjami.",
};

export default function RootLayout({ children }: PropsWithChildren) {
	return (
		<html lang="en" data-theme="ezhire">
			<body className={`${geistSans.variable} ${geistMono.variable} antialiased bg-base-200`}>{children}</body>
		</html>
	);
}
