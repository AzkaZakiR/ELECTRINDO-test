import { useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";

export default function App() {
  const [machines, setMachines] = useState({});

  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:5093/machineHub", {
        withCredentials: true,
      })
      .withAutomaticReconnect()
      .build();

    let isMounted = true;

    connection.on("ReceiveMachineUpdate", (data) => {
      console.log("Machine Update:", data);
      if (!isMounted) return;

      setMachines((prev) => ({
        ...prev,
        [data.machine]: {
          status: data.status,
          itemsPerMinute: data.itemsPerMinute,
          temperature: data.temperature,
          operator: data.operatorName,
          lastUpdate: data.createdAt,
        },
      }));
    });

    async function startConnection() {
      try {
        await connection.start();

        if (isMounted) {
          console.log("SignalR Connected");
        }
      } catch (err) {
        console.error(err);
      }
    }

    startConnection();

    return () => {
      isMounted = false;
      connection.stop();
    };
  }, []);

  const getStatusColor = (status) => {
    switch (status) {
      case "Running":
        return "green";
      case "Idle":
        return "orange";
      case "Maintenance":
        return "blue";
      case "Error":
        return "red";
      default:
        return "gray";
    }
  };

  return (
    <div style={{ padding: 20, fontFamily: "Arial" }}>
      <h2> Dashboard Monitoring Mesin Produksi</h2>

      <table border="1" cellPadding="10" style={{ width: "100%", marginTop: 20 }}>
        <thead>
          <tr>
            <th>Machine</th>
            <th>Status</th>
            <th>Items / Min</th>
            <th>Temperature</th>
            <th>Operator</th>
            <th>Last Update</th>
          </tr>
        </thead>

        <tbody>
          {Object.keys(machines).length === 0 ? (
            <tr>
              <td colSpan="6" style={{ textAlign: "center" }}>
                Waiting for data...
              </td>
            </tr>
          ) : (
            Object.entries(machines).map(([name, m]) => (
              <tr key={name}>
                <td>{name}</td>

                <td style={{ color: getStatusColor(m.status), fontWeight: "bold" }}>
                  ● {m.status}
                </td>

                <td>{m.itemsPerMinute}</td>

                <td>{m.temperature} °C</td>

                <td>{m.operator}</td>

                <td>
                  {m.lastUpdate
                    ? new Date(m.lastUpdate).toLocaleTimeString()
                    : "-"}
                </td>
              </tr>
            ))
          )}
        </tbody>
      </table>
    </div>
  );
}