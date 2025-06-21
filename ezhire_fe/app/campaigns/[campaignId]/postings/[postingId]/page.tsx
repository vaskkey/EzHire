"use client";

import { ApplicantStatus, client, JobPostingObject, PostingStatus, UserType } from "@/api/client";
import { UserContext } from "@/app/context/auth";
import { useRouter } from "next/navigation";
import { useContext, useEffect, useMemo, useState } from "react";

export default function Posting({ params }: { params: Promise<{ postingId: number }> }) {
	const currentUser = useContext(UserContext);
	const [posting, setPosting] = useState<JobPostingObject>();
	const [showToast, setShowToast] = useState(false);
	const [isLoading, setLoading] = useState(true);
	const router = useRouter();

	const postingStatus = new Map([
		[PostingStatus.OPEN, "badge-success"],
		[PostingStatus.CLOSED, "badge-error"],
	]);

	const postingStatusName = new Map([
		[PostingStatus.OPEN, "Otwarte"],
		[PostingStatus.CLOSED, "Zamknięte"],
	]);

	const badgeStatus = new Map([
		[ApplicantStatus.APPLIED, "badge-warning"],
		[ApplicantStatus.REJECTED, "badge-error"],
		[ApplicantStatus.ACCEPTED, "badge-success"],
		[ApplicantStatus.PENDING, "badge-info"],
	]);

	const statusName = new Map([
		[ApplicantStatus.APPLIED, "Zaaplikowano"],
		[ApplicantStatus.REJECTED, "Odmowa"],
		[ApplicantStatus.ACCEPTED, "Zaakceptowano"],
		[ApplicantStatus.PENDING, "W Trakcie"],
	]);

	const isManager = useMemo(() => currentUser?.userType === UserType.HIRING_MANAGER, [currentUser]);

	async function closePosting() {
		const prms = await params;
		const { error } = await client.POST("/api/postings/{id}/close", { params: { path: { id: prms.postingId } } });

		if (error) {
			setShowToast(true);
			setTimeout(() => setShowToast(false), 1000);
			return;
		}

		await fetchPosting();
	}

	async function fetchPosting() {
		setLoading(true);
		const prms = await params;

		const { data, error } = await client.GET("/api/postings/{id}", { params: { path: { id: prms.postingId } } });

		setLoading(false);

		if (error) {
			router.replace("/campaigns");
			return;
		}

		setPosting(data);
	}

	useEffect(() => {
		fetchPosting();
	}, []);

	return (
		<div>
			{isLoading && ""}
			{!isLoading && (
				<>
					<div className="flex justify-between">
						<div>
							<h2 className="font-extrabold text-3xl mb-4">{posting?.jobName}</h2>
							<div className="flex flex-row gap-4">
								<h5 className="text-xs flex gap-3 items-center">
									Utworzono:
									{posting?.datePosted && (
										<div className="font-bold">{new Date(posting?.datePosted).toLocaleString()}</div>
									)}
								</h5>
								<h5 className="text-xs flex gap-3 items-center">
									Kampania:
									<div className="font-bold">{posting?.campaign.name}</div>
								</h5>
								<h5 className="text-xs flex gap-3 items-center">
									Status:
									<span
										className={`badge badge-sm badge-outline ${postingStatus.get(posting?.status ?? PostingStatus.OPEN)}`}
									>
										{postingStatusName.get(posting?.status ?? PostingStatus.OPEN)}
									</span>
								</h5>
							</div>
						</div>
						{isManager && posting?.status === PostingStatus.OPEN && (
							<button className="btn btn-error text-white" onClick={closePosting}>
								Zamknij Rekrutację
							</button>
						)}
					</div>
					<div className="overflow-x-auto mt-20">
						<table className="table">
							<thead>
								<tr>
									<th>Imię</th>
									<th>Data Aplikacji</th>
									<th>Status</th>
								</tr>
							</thead>
							<tbody>
								{posting?.applications.map((application) => (
									<tr key={application.id}>
										<td>
											{application.applicant && `${application.applicant.firstName} ${application.applicant.lastName}`}
										</td>
										<td>{new Date(application.dateApplied).toLocaleString()}</td>
										<td>
											<span
												className={`badge badge-xs badge-soft ${badgeStatus.get(application?.status ?? ApplicantStatus.APPLIED)}`}
											>
												{statusName.get(application?.status ?? ApplicantStatus.APPLIED)}
											</span>
										</td>
									</tr>
								))}
							</tbody>
						</table>
					</div>
				</>
			)}
			{showToast && (
				<div className="toast toast-start z-50">
					<div className="alert alert-error">
						<span>Nie udało się zamknąć rekrutacji.</span>
					</div>
				</div>
			)}
		</div>
	);
}
