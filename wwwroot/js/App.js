import Header from "../components/Header";
import Sidebar from "../components/Sidebar";
import {Col, Container, Row} from "react-bootstrap";
import {Switch, Route} from "react-router-dom";
import MainPage from "../pages/MainPage";
import AuthPage from "../pages/AuthPage";
import ArticlesListPage from "../pages/ArticlesListPage";
import ArticlePage from "../pages/ArticlePage";
import AdminPage from "../pages/AdminPage";
import UserPage from "../pages/UserPage";


function App() {
    return (
        <div>
            <Header/>
            <Row>
                <Col xs={3}>
                    <Sidebar location={window.location}/>
                    <LeftWidget/>
                </Col>
                <Col xs={9}>
                    <Switch>
                        <Route exact path={"/"} component={MainPage}/>
                        <Route exact path={"/Auth"} component={AuthPage}/>
                        <Route exact path={"/Articles"} component={ArticlesListPage}/>
                        <Route exact path={"/Article/:id"} component={ArticlePage}/>
                        <Route exact path={"/Admin"} component={AdminPage}/>
                        <Route exact path={"/User"} component={UserPage}/>
                    </Switch>
                </Col>
            </Row>
        </div>
    )
}

export default App