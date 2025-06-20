import type { Metadata } from "next";
import { PropsWithChildren } from "react";
import { Megaphone, Moon, Sun } from "lucide-react";
import Link from "next/link";
import { Auth } from "@/components/Auth";

export const metadata: Metadata = {
	title: "EzHire - Kampanie",
	description: "Zarządzaj swoimi kampaniami",
};

export default function RootLayout({ children }: PropsWithChildren) {
	return (
		<Auth>
			<div className="drawer lg:drawer-open">
				<input id="my-drawer" type="checkbox" className="drawer-toggle" checked readOnly />
				<div className="drawer-content">
					<div className="navbar lg:px-6 px-2 flex justify-between">
						<div className="breadcrumbs text-sm">
							<ul>
								<li>
									<a>Kampanie</a>
								</li>
								<li>
									<a>Rekrutacje</a>
								</li>

								<li>Frontend Developer (Ember.js)</li>
							</ul>
						</div>
						<label className="swap swap-rotate">
							{/* this hidden checkbox controls the state */}
							<input type="checkbox" className="theme-controller" value="coolpuff" />

							<Sun className="swap-off" size={18} />

							<Moon className="swap-on" size={18} />
						</label>
					</div>
					<main className="bg-base-200 min-h-screen lg:p-6 p-2">
						<div className="bg-background rounded-md min-h-full p-5">{children}</div>
					</main>
				</div>
				<aside className="drawer-side bg-green-50">
					<label htmlFor="my-drawer" aria-label="close sidebar" className="drawer-overlay"></label>
					<ul className="menu bg-background text-base-content min-h-full w-80 p-4">
						<h2 className="text-2xl font-bold text-blue-600 mb-4">EzHire</h2>
						<li>
							<Link href="/campaigns">
								<Megaphone size={18} />
								Kampanie
							</Link>
						</li>
					</ul>
				</aside>
			</div>
		</Auth>
	);
}
