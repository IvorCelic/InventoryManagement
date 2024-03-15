import { useEffect, useState } from "react";
import { Button, Container, Table } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import { FaEdit, FaTrash } from "react-icons/fa";
import WarehouseService from "../../services/WarehouseService";
import { RoutesNames } from "../../constants";
import SearchAndAdd from "../../components/SearchAndAdd";

export default function Warehouses() {
    const [warehouses, setWarehouses] = useState();
    const navigate = useNavigate();

    async function fetchWarehouses() {
        await WarehouseService.get()
            .then((res) => {
                setWarehouses(res.data);
            })
            .catch((error) => {
                alert(error);
            });
    }

    useEffect(() => {
        fetchWarehouses();
    }, []);

    async function removeWarehouse(id) {
        const response = await WarehouseService.remove(id);
        if (response.ok) {
            alert(response.message.data.message);
            fetchWarehouses();
        }
    }

    return (
        <Container>
            <Container>
                <SearchAndAdd RouteName={RoutesNames.WAREHOUSES_CREATE} entity={"warehouse"} />
            </Container>
            <Table striped bordered hover responsive>
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Description</th>
                        <th>Action</th>
                    </tr>
                </thead>
                <tbody>
                    {warehouses &&
                        warehouses.map((warehouse, index) => (
                            <tr key={index}>
                                <td>{warehouse.warehouseName}</td>
                                <td>{warehouse.description}</td>
                                <td>
                                    <Container className="d-flex justify-content-center">
                                        <Button
                                            variant="link"
                                            className="me-2 actionButton"
                                            onClick={() => {
                                                navigate(`/warehouses/${warehouse.id}`);
                                            }}
                                        >
                                            <FaEdit size={25} />
                                        </Button>
                                        <Button
                                            className="link-danger actionButton"
                                            onClick={() => removeWarehouse(warehouse.id)}
                                        >
                                            <FaTrash size={25} />
                                        </Button>
                                    </Container>
                                </td>
                            </tr>
                        ))}
                </tbody>
            </Table>
        </Container>
    );
}
