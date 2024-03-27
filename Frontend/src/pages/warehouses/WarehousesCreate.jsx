import { Button, Col, Container, Form, Row } from "react-bootstrap";
import { Link, useNavigate } from "react-router-dom";
import { RoutesNames } from "../../constants";
import WarehouseService from "../../services/WarehouseService";

export default function WarehousesCreate() {
    const navigate = useNavigate();
    const entityName = "warehouse";

    async function addWarehouse(entityName) {
        const response = await WarehouseService.add(entityName);
        if (response.ok) {
            navigate(RoutesNames.WAREHOUSES_LIST);
        } else {
            console.log(response);
            alert(response.message);
        }
    }

    function handleSubmit(error) {
        error.preventDefault();
        const data = new FormData(error.target);

        const entityName = {
            warehouseName: data.get("warehousename"),
            description: data.get("description"),
        };

        addWarehouse(entityName);
    }

    return (
        <Container className="square border mt-5">
            <h2 className="mt-5 ms-5">Add new {entityName}</h2>
            <Form className="m-5" onSubmit={handleSubmit}>
                <Form.Group controlId="warehousename">
                    <Form.Label>Name</Form.Label>
                    <Form.Control placeholder="Warehouse name" type="text" name="warehousename" />
                </Form.Group>
                <Form.Group controlId="description">
                    <Form.Label className="pt-4">Description</Form.Label>
                    <Form.Control
                        placeholder="Warehouse description"
                        type="text"
                        name="description"
                    />
                </Form.Group>

                <Row className="mb-0 flex-column flex-sm-row">
                    <Col className="d-flex align-items-center mb-2 mb-sm-0">
                        <Link className="btn btn-danger myButton" to={RoutesNames.WAREHOUSES_LIST}>
                            Cancel
                        </Link>
                    </Col>
                    <Col className="d-flex align-items-center">
                        <Button className="myButton" variant="primary" type="submit">
                            Add {entityName}
                        </Button>
                    </Col>
                </Row>
            </Form>
        </Container>
    );
}
