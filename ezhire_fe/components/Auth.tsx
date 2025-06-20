"use client";

import { client, User } from "@/api/client";
import { UserContext } from "@/app/context/auth";
import { redirect, RedirectType } from "next/navigation";
import { PropsWithChildren, useEffect, useState } from "react";

export function Auth({ children }: PropsWithChildren) {
	const [user, setUser] = useState<User | null>(null);
	const [isLoading, setLoading] = useState(true);

	async function getMe() {
		const { data, error, response } = await client.GET("/api/auth/me");

		if (error || response.status === 401) {
			redirect("/auth/login", RedirectType.replace);
			return;
		}
		setUser(data);
		setLoading(false);
	}

	useEffect(() => {
		getMe();
	}, []);

	return (
		<>
			{isLoading && <span className="loading loading-dots loading-xl"></span>}

			{!isLoading && <UserContext value={user}>{children}</UserContext>}
		</>
	);
}
