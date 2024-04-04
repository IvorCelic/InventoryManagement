import { Container, Form } from "react-bootstrap";
import { useNavigate, useParams } from "react-router-dom";
import { RoutesNames } from "../../constants";
import WarehouseService from "../../services/WarehouseService";
import { useEffect, useState } from "react";
import useError from "../../hooks/useError";
import ActionButtons from "../../components/ActionButtons";

export default function WarehousesEdit() {
    const navigate = useNavigate();
    const routeParams = useParams();
    const [warehouse, setWarehouse] = useState({});
    const entityName = "warehouse";
    const { showError } = useError();
    const [showModal, setShowModal] = useState();

    async function fetchWarehouse() {
        const response = await WarehouseService.getById("Warehouse", routeParams.id);
        if (!response.ok) {
            showError(response.data);
            navigate(RoutesNames.WAREHOUSES_LIST);
            return;
        }
        setWarehouse(response.data);
        setShowModal(false);
    }

    useEffect(() => {
        fetchWarehouse();
    }, []);

    async function editWarehouse(entityName) {
        const response = await WarehouseService.edit("Warehouse", routeParams.id, entityName);
        if (response.ok) {
            navigate(RoutesNames.WAREHOUSES_LIST);
            return;
        }
        showError(response.data);
    }

    function handleSubmit(event) {
        event.preventDefault();
        const data = new FormData(event.target);

        editWarehouse({
            warehouseName: data.get("warehousename"),
            description: data.get("description"),
        });
    }

    return (
        <Container>
            <Container className="square border mt-5">
                <h2 className="mt-5 ms-5">Edit {entityName}</h2>
                <Form className="m-5" onSubmit={handleSubmit}>
                    <Form.Group controlId="warehouseName">
                        <Form.Label>Warehouse Name</Form.Label>
                        <Form.Control
                            type="text"
                            defaultValue={warehouse.warehouseName}
                            name="warehousename"
                        />
                    </Form.Group>
                    <Form.Group controlId="description">
                        <Form.Label className="pt-4">Description</Form.Label>
                        <Form.Control
                            type="text"
                            defaultValue={warehouse.description}
                            name="description"
                        />
                    </Form.Group>
                    <ActionButtons cancel={RoutesNames.WAREHOUSES_LIST} action="Edit warehouse" />
                </Form>
            </Container>
        </Container>
    );
}
