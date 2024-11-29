import React from 'react';
import ReactDOM from 'react-dom/client';
import './Index.css';
import App from './App';

const Index = () => {
    const [posts, setPosts] = useState([]);

    useEffect(() => {
        fetch('/api/posts') // Endre til riktig API-endepunkt
            .then(response => response.json())
            .then(data => setPosts(data))
            .catch(error => console.error('Error fetching posts:', error));
    }, []);

    return (
        <div className="home-page">
            <div className="profile-container-left">
                {/* Venstre seksjon kan fylles ut med ønsket innhold */}
            </div>

            <div className="profile-container-right">
                {/* Brukerens innlegg */}
                {posts.map((post) => (
                    <div className="container-post" key={post.id}>
                        <div className="post-header">
                            <div className="post-pp">
                                <img
                                    alt="Profile"
                                    className="pp-content"
                                    src={post.user?.profilePic || '/images/default-profile.png'}
                                />
                            </div>
                            <div className="post-profile">
                                {post.user ? post.user.userName : <span>Unknown User</span>}
                            </div>
                            <div className="post-posted">
                                <span>
                                    {new Date(post.createdAt).toLocaleDateString()} <br />
                                    at {new Date(post.createdAt).toLocaleTimeString()}
                                </span>
                            </div>
                        </div>

                        <div className="post-contents">
                            <p>{post.content}</p>
                            {post.imageUrl && (
                                <img
                                    className="photo"
                                    src={post.imageUrl}
                                    alt="Post content"
                                />
                            )}

                            <div className="post-interaction">
                                <span className="post-choice">
                                    <a href="#">
                                        <i className="fa-regular fa-heart"></i> Like
                                    </a>
                                </span>
                                <span className="post-choice">
                                    <a href="#">
                                        <i className="fa-regular fa-comment"></i> Comment
                                    </a>
                                </span>
                                <span className="post-choice">
                                    <a href="#">
                                        <i className="fa-regular fa-star"></i> Favorite
                                    </a>
                                </span>
                                <span className="post-choice">
                                    <a href="#">
                                        <i className="fa-regular fa-share-from-square"></i> Share
                                    </a>
                                </span>
                            </div>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default Index;
