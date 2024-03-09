import { Button, Container, Table } from "react-bootstrap";
import { RoutesNames } from "../../constants";
import { useNavigate } from "react-router-dom";
import { FaEdit, FaTrash } from "react-icons/fa";
import SearchAndAdd from "../../components/SearchAndAdd";
import { useEffect, useState } from "react";
import PersonService from "../../services/PersonService";

export default function Persons() {
    const [Persons, setPersons] = useState();
    let navigate = useNavigate();

    async function fetchPersons() {
        await PersonService.get()
        .then((res) => {
            setPersons(res.data);
        })
        .catch((error) => {
            alert(error);
        });
    }

    useEffect(() => {
        fetchPersons();
    },[]);

    async function handlePersonDelete(id) {
        const response = await PersonService.remove(id);

        if (response.ok) {
            alert(response.message.data.message);
            fetchPersons();
        }
        else {
            alert(response.message);
        }
    }

    return (
        <Container>
            <Container>
                <SearchAndAdd RouteName={RoutesNames.PERSONS_CREATE} entity={"person"} />
            </Container>
            <Table striped bordered hover responsive>
                <thead>
                    <tr>
                        <th>First name</th>
                        <th>Last name</th>
                        <th>Action</th>
                    </tr>
                </thead>
                <tbody>
                    {Persons && Persons.map((person, index) => (
                        <tr key={index}>
                            <td>{person.firstName}</td>
                            <td>{person.lastName}</td>
                            <td >
                                <Container
                                    className="d-flex justify-content-center"
                                >
                                    <Button
                                        variant="link"
                                        className="me-2 actionButton"
                                        onClick={() => { navigate(`/persons/${person.id}`) }}
                                    >
                                        <FaEdit
                                            size={25}
                                        />
                                    </Button>
                                    <Button
                                        className='link-danger actionButton'
                                        onClick={() => handlePersonDelete(person.id)}
                                    >
                                        <FaTrash
                                            size={25}
                                        />
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