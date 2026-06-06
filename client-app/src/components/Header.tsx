import ".././styles.css";

export default function Header() {
  //   const hello = "hello header";

  return (
    <header className="navbar navbar-expand-lg">
      <div className="container">
        <a className="navbar-brand">iREDO</a>

        <div className="navbar-nav ms-auto">
          <a className="nav-link">Catalogue</a>
          <a className="nav-link">About</a>
        </div>
      </div>
    </header>
  );
}
