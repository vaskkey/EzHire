export default function Home() {
  const applicants = [
    { id: 1, name: "Bruce Wayne", date: "27-07-2025", status: "applied" },
    { id: 2, name: "John Constantine", date: "28-07-2025", status: "rejected" },
    { id: 3, name: "Jessie Custer", date: "29-07-2025", status: "accepted" },
  ];

  const badgeStatus = new Map([
    ["applied", "badge-warning"],
    ["rejected", "badge-error"],
    ["accepted", "badge-success"],
  ]);

  const statusName = new Map([
    ["applied", "Zaaplikowano"],
    ["rejected", "Odmowa"],
    ["accepted", "Zaakceptowano"],
  ]);

  return (
    <div>
      <div className="flex justify-between">
        <div>
          <h2 className="font-extrabold text-3xl mb-4">
            Frontend Developer (Ember.js)
          </h2>
          <div className="flex flex-row gap-4">
            <h5 className="text-xs flex gap-3 items-center">
              Utworzono:
              <div className="font-bold">25-06-2025</div>
            </h5>
            <h5 className="text-xs flex gap-3 items-center">
              Kampania:
              <div className="font-bold">Chat AI - Nowy zespół</div>
            </h5>
            <h5 className="text-xs flex gap-3 items-center">
              Status:
              <span className="badge badge-xs badge-outline badge-success">
                Otwarte
              </span>
            </h5>
          </div>
        </div>
        <button className="btn btn-error text-white">Zamknij Rekrutację</button>
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
            {applicants.map((applicant) => (
              <tr key={applicant.id}>
                <td>{applicant.name}</td>
                <td>{applicant.date}</td>
                <td>
                  <span
                    className={`badge badge-xs badge-soft ${badgeStatus.get(applicant.status) ?? ""}`}
                  >
                    {statusName.get(applicant.status)}
                  </span>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}
